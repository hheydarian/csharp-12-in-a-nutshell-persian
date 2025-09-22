# فصل هجدهم:  بازتاب (Reflection) و متادیتا

همان‌طور که در فصل ۱۷ دیدیم، یک برنامه‌ی C# به یک **Assembly** کامپایل می‌شود که شامل **متادیتا (Metadata)**، کد کامپایل‌شده و منابع (Resources) است. بررسی متادیتا و کد کامپایل‌شده در زمان اجرا را **Reflection (بازتاب)** می‌نامند.

کد کامپایل‌شده در یک Assembly تقریباً تمام محتوای کد منبع اصلی را در بر دارد. با این حال، برخی اطلاعات مانند نام متغیرهای محلی، توضیحات (Comments) و دستورهای پیش‌پردازنده (Preprocessor Directives) از دست می‌روند. اما بازتاب به ما امکان دسترسی به تقریباً تمام موارد دیگر را می‌دهد—حتی تا حدی که می‌توان یک **Decompiler** (دی‌کامپایلر) نوشت. 🔎

بسیاری از سرویس‌های موجود در .NET و در دسترس از طریق C# (مانند **Dynamic Binding**، **Serialization** و **Data Binding**) به وجود متادیتا وابسته هستند. همچنین برنامه‌های شما نیز می‌توانند از این متادیتا استفاده کنند و حتی آن را با اطلاعات جدید از طریق **Custom Attributes** گسترش دهند. فضای نام `System.Reflection` شامل API مربوط به Reflection است. علاوه بر این، در زمان اجرا می‌توان متادیتا و دستورالعمل‌های اجرایی جدیدی در سطح **Intermediate Language (IL)** با استفاده از کلاس‌های موجود در فضای نام `System.Reflection.Emit` ایجاد کرد.

نمونه‌های این فصل فرض می‌کنند که شما فضای نام‌های `System` و `System.Reflection` و همچنین `System.Reflection.Emit` را وارد کرده‌اید.

وقتی در این فصل از اصطلاح **«به‌صورت دینامیکی» (Dynamically)** استفاده می‌کنیم، منظور این است که عملی را با Reflection انجام دهیم که **ایمنی نوع (Type Safety)** آن فقط در زمان اجرا کنترل می‌شود. این موضوع از نظر اصول مشابه **Dynamic Binding** در C# با کلیدواژه‌ی `dynamic` است، اما مکانیزم و عملکرد آن متفاوت است.

* **Dynamic Binding** استفاده‌ی آسان‌تری دارد و از **Dynamic Language Runtime (DLR)** برای سازگاری با زبان‌های پویا استفاده می‌کند.
* **Reflection** نسبت به آن کمی دست‌وپاگیرتر است، اما انعطاف بیشتری در ارتباط با **CLR** ارائه می‌دهد.

برای مثال، Reflection به شما اجازه می‌دهد:
✔️ فهرستی از **Types** و **Members** دریافت کنید.
✔️ یک شیء را با نامی که از یک رشته (String) می‌آید بسازید.
✔️ در لحظه (On the fly) Assembly تولید کنید.

---

## 🔍 Reflecting and Activating Types

در این بخش بررسی می‌کنیم که چگونه می‌توان یک **Type** را به دست آورد، متادیتای آن را بررسی کرد و از آن برای ایجاد دینامیکی یک شیء استفاده نمود.

### 📌 Obtaining a Type

یک نمونه از `System.Type` نمایانگر متادیتای یک Type است. از آن‌جا که **Type** بسیار پرکاربرد است، در فضای نام `System` قرار دارد، نه در `System.Reflection`.

روش‌های به‌دست‌آوردن یک نمونه‌ی `System.Type`:

۱. فراخوانی متد `GetType` روی هر شیء:

```csharp
Type t1 = DateTime.Now.GetType();     // Type بدست‌آمده در زمان اجرا
```

۲. استفاده از عملگر `typeof` در C#:

```csharp
Type t2 = typeof(DateTime);          // Type بدست‌آمده در زمان کامپایل
```

با استفاده از `typeof` می‌توانید Type آرایه‌ها و Typeهای جنریک را نیز بگیرید:

```csharp
Type t3 = typeof(DateTime[]);          // آرایه یک‌بعدی
Type t4 = typeof(DateTime[,]);         // آرایه دوبعدی
Type t5 = typeof(Dictionary<int,int>); // جنریک بسته (Closed Generic Type)
Type t6 = typeof(Dictionary<,>);       // جنریک باز (Unbound Generic Type)
```

۳. دریافت Type از طریق نام (Name):
اگر یک مرجع به Assembly داشته باشید:

```csharp
Type t = Assembly.GetExecutingAssembly().GetType("Demos.TestProgram");
```

اگر Assembly را نداشته باشید، می‌توانید از **Assembly Qualified Name** استفاده کنید (نام کامل Type به‌همراه نام کامل یا جزئی Assembly). در این حالت Assembly به‌طور ضمنی بارگذاری می‌شود:

```csharp
Type t = Type.GetType("System.Int32, System.Private.CoreLib");
```

پس از در اختیار داشتن یک شیء `System.Type`، می‌توانید با استفاده از ویژگی‌های آن به اطلاعاتی مانند نام، Assembly، Base Type، سطح دسترسی (Visibility) و ... دسترسی داشته باشید:

```csharp
Type stringType = typeof(string);
string name     = stringType.Name;          // String
Type baseType   = stringType.BaseType;      // typeof(Object)
Assembly assem  = stringType.Assembly;      // System.Private.CoreLib
bool isPublic   = stringType.IsPublic;      // true
```

یک شیء از نوع `System.Type` در واقع پنجره‌ای به تمام متادیتای مربوط به آن Type و Assembly حاوی آن است.

> `System.Type` یک کلاس **Abstract** است، بنابراین عملگر `typeof` در واقع یک زیرکلاس از Type را برمی‌گرداند. زیرکلاسی که CLR استفاده می‌کند داخلی (Internal) بوده و نام آن **RuntimeType** است.

---

## 📘 TypeInfo

اگر شما هدف‌گذاری روی **.NET Core 1.x** (یا پروفایل‌های قدیمی‌تر Windows Store) داشته باشید، بسیاری از اعضای `Type` در دسترس نیستند. این اعضا به جای آن در کلاسی به نام `TypeInfo` ارائه می‌شوند که از طریق فراخوانی `GetTypeInfo` به‌دست می‌آید.

برای اجرای مثال قبلی در چنین محیطی، کد شما این‌گونه خواهد بود:

```csharp
Type stringType = typeof(string);
string name = stringType.Name;
Type baseType = stringType.GetTypeInfo().BaseType;
Assembly assem = stringType.GetTypeInfo().Assembly;
bool isPublic = stringType.GetTypeInfo().IsPublic;
```

کلاس `TypeInfo` در **.NET Core 2 و 3** و **.NET 5+** (و همچنین در **.NET Framework 4.5+** و تمامی نسخه‌های **.NET Standard**) نیز وجود دارد. بنابراین کد بالا تقریباً به‌طور جهانی (Universal) قابل اجراست.

همچنین `TypeInfo` ویژگی‌ها و متدهای اضافی برای بازتاب روی اعضا (Reflecting over Members) در اختیار قرار می‌دهد.


