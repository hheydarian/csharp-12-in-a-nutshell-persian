# فصل نوزدهم:  برنامه‌نویسی پویا (Dynamic Programming)

فصل ۴ توضیح داد که **dynamic binding** در زبان C# چگونه کار می‌کند.
در این فصل، ابتدا به‌طور مختصر به **Dynamic Language Runtime (DLR)** می‌پردازیم و سپس الگوهای زیر در برنامه‌نویسی پویا را بررسی می‌کنیم:

* **Dynamic member overload resolution**
* **Custom binding (implementing dynamic objects)**
* **Dynamic language interoperability**

در فصل ۲۴، توضیح می‌دهیم که **dynamic** چگونه می‌تواند **COM interoperability** را بهبود دهد.

انواع (types) معرفی‌شده در این فصل، در **System.Dynamic namespace** قرار دارند، به‌جز **CallSite<>** که در **System.Runtime.CompilerServices** تعریف شده است.

---

### 🌀 Dynamic Language Runtime

زبان C# برای انجام **dynamic binding** به **DLR** متکی است.

برخلاف نامش، **DLR** یک نسخه‌ی پویا از **CLR** نیست. بلکه یک **کتابخانه (library)** است که روی **CLR** قرار می‌گیرد—دقیقاً مانند هر کتابخانه‌ی دیگری مثل **System.Xml.dll**.

وظیفه‌ی اصلی DLR ارائه‌ی سرویس‌های زمان اجرا (runtime services) برای یکپارچه‌سازی برنامه‌نویسی پویا—چه در زبان‌های **statically typed** و چه **dynamically typed**—است. بنابراین، زبان‌هایی مانند:

* C#
* Visual Basic
* IronPython
* IronRuby

همگی از یک پروتکل یکسان برای **dynamic function calls** استفاده می‌کنند. این موضوع باعث می‌شود آن‌ها بتوانند کتابخانه‌ها را به اشتراک بگذارند و کدی را که به زبان دیگری نوشته شده اجرا کنند.

DLR همچنین نوشتن زبان‌های پویا جدید در **.NET** را نسبتاً آسان می‌کند. به‌جای آنکه نویسندگان زبان مجبور باشند مستقیم **Intermediate Language (IL)** تولید کنند، می‌توانند در سطح **expression trees** کار کنند (همان **expression trees** موجود در **System.Linq.Expressions** که در فصل ۸ درباره‌شان صحبت کردیم).

علاوه بر این، DLR تضمین می‌کند که همه‌ی مصرف‌کنندگان از مزیت **call-site caching** بهره‌مند شوند؛ یک بهینه‌سازی که از تکرار غیرضروری تصمیمات پرهزینه‌ی **member resolution** در طول dynamic binding جلوگیری می‌کند.

---

### ❓ Call Site چیست؟

وقتی کامپایلر با یک **dynamic expression** روبه‌رو می‌شود، نمی‌داند چه کسی آن عبارت را در زمان اجرا ارزیابی خواهد کرد.

مثلاً متد زیر را در نظر بگیرید:

```csharp
public dynamic Foo (dynamic x, dynamic y)
{
  return x / y;   // Dynamic expression
}
```

متغیرهای `x` و `y` می‌توانند هر چیزی باشند:

* یک شیء **CLR**
* یک شیء **COM**
* یا حتی یک شیء در یک زبان پویا

به همین دلیل، کامپایلر نمی‌تواند از روش معمول استاتیک خود (یعنی صدا زدن یک متد مشخص از یک نوع مشخص) استفاده کند.
در عوض، کامپایلر کدی تولید می‌کند که در نهایت یک **expression tree** می‌سازد؛ این **expression tree** عملیاتی را توصیف می‌کند که توسط یک **call site** مدیریت می‌شود و **DLR** آن را در زمان اجرا bind می‌کند.
درواقع، **call site** مانند یک واسطه (intermediary) بین **caller** و **callee** عمل می‌کند.

یک **call site** توسط کلاس **CallSite<>** در **System.Core.dll** نمایش داده می‌شود.
با **disassemble** کردن متد قبلی، نتیجه تقریباً به‌شکل زیر خواهد بود:

```csharp
static CallSite<Func<CallSite,object,object,object>> divideSite;

[return: Dynamic]
public object Foo ([Dynamic] object x, [Dynamic] object y)
{
  if (divideSite == null)
    divideSite =
      CallSite<Func<CallSite,object,object,object>>.Create (
        Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation (
          CSharpBinderFlags.None,
          ExpressionType.Divide,
          /* Remaining arguments omitted for brevity */ ));

  return divideSite.Target (divideSite, x, y);
}
```

همان‌طور که می‌بینید، **call site** در یک **static field** ذخیره می‌شود تا هزینه‌ی ساخت مجدد آن در هر بار فراخوانی اجتناب شود.
همچنین، DLR نتیجه‌ی **binding phase** و **method targets** واقعی را cache می‌کند. (ممکن است چندین target بسته به نوع‌های `x` و `y` وجود داشته باشد.)

فراخوانی پویا (dynamic call) در عمل با صدا زدن **Target** (که یک **delegate** است) انجام می‌شود و عملوندهای `x` و `y` به آن پاس داده می‌شوند.

نکته‌ی مهم: کلاس **Binder** مخصوص هر زبان است.
هر زبانی که از **dynamic binding** پشتیبانی می‌کند، یک **language-specific binder** دارد تا به DLR کمک کند عبارات را مطابق منطق آن زبان تفسیر کند و رفتار غیرمنتظره برای برنامه‌نویس ایجاد نشود.

مثلاً اگر متد `Foo` را با مقادیر عددی `5` و `2` صدا بزنیم:

* **C# binder** نتیجه‌ی `2` را برمی‌گرداند.
* اما **VB.NET binder** نتیجه‌ی `2.5` را خواهد داد.

---

### ⚡ Dynamic Member Overload Resolution

فراخوانی یک متد **statically known** با آرگومان‌های **dynamically typed** باعث می‌شود که **member overload resolution** از زمان کامپایل به زمان اجرا منتقل شود.

این ویژگی برای ساده‌سازی برخی وظایف برنامه‌نویسی مفید است—مثل ساده‌تر کردن **Visitor design pattern**.
همچنین در دور زدن محدودیت‌های اعمال‌شده توسط **static typing** در C# بسیار کاربرد دارد.

