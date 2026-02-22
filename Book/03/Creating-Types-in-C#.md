<div dir="rtl">

# فصل سوم : ایجاد Typeها در سی شارپ

در این فصل، ما وارد مبحث **typeها** و **type memberها** می‌شویم.

---

### 📌 کلاس‌ها (Classes)

یک **class** رایج‌ترین نوع از **reference type** است. ساده‌ترین شکل ممکن برای تعریف یک کلاس به این صورت است:

```csharp
class YourClassName
{
}
```

یک کلاس پیچیده‌تر می‌تواند شامل موارد زیر باشد:

* **قبل از کلمه کلیدی class** → می‌تواند شامل **attributes** و **class modifiers** باشد.
* **بعد از نام کلاس (YourClassName)** → می‌تواند شامل **generic type parameters** باشد.
* **داخل براکت‌ها { }** → می‌تواند شامل **class members** مانند موارد زیر باشد:

  * fields
  * constructors
  * methods
  * properties
  * indexers
  * events
  * finalizer
  * overloaded operators
  * nested types
  * base class
  * interfaces
  * constraints

⚠️ در این فصل، همه این ساختارها توضیح داده می‌شوند، **به‌جز** موارد زیر که در فصل ۴ پوشش داده خواهند شد:

* attributes
* operator functions
* کلمه کلیدی `unsafe`

بخش‌های بعدی، هر یک از **class memberها** را بررسی می‌کنند.

---

### 📝 فیلدها (Fields)

یک **field** در واقع یک متغیر است که به‌عنوان عضوی از یک **class** یا **struct** تعریف می‌شود؛ برای مثال:

```csharp
class Octopus
{
    string name;
    public int Age = 10;
}
```

---

#### 🔑 فیلدها می‌توانند شامل این modifiers باشند:

* **Static modifier** → `static`
* **Access modifiers** → `public`, `internal`, `private`, `protected`
* **Inheritance modifier** → `new`
* **Unsafe code modifier** → `unsafe`
* **Read-only modifier** → `readonly`
* **Threading modifier** → `volatile`

---

#### 🐪 قراردادهای نام‌گذاری (Naming Conventions) برای فیلدهای private

دو قرارداد پرکاربرد وجود دارد:

1. **camelCase** → مثل: `firstName`
2. **camelCase همراه با underscore** → مثل: `_firstName`

روش دوم کمک می‌کند که سریعاً فیلدهای private را از **پارامترها** و **متغیرهای محلی** تشخیص دهید.

---

#### 🔒 کلمه کلیدی `readonly`

کلمه کلیدی **`readonly`** باعث می‌شود یک فیلد پس از ساخت (construction) دیگر قابل تغییر نباشد.
یک فیلد **readonly** فقط می‌تواند در **اعلان آن** یا در **سازنده‌ی (constructor) همان type** مقداردهی شود.

---

#### ⚙️ مقداردهی اولیه فیلدها (Field Initialization)

* مقداردهی اولیه برای فیلدها **اختیاری** است.
* اگر یک فیلد مقداردهی نشود، مقدار پیش‌فرض خواهد داشت:

  * عددی‌ها: `0`
  * کاراکتر: `'\0'`
  * reference types: `null`
  * bool: `false`

📌 مقداردهی اولیه فیلدها **قبل از constructorها** اجرا می‌شود:

```csharp
public int Age = 10;
```

مقداردهی اولیه می‌تواند شامل **عبارت‌ها** یا حتی **فراخوانی متدها** باشد:

```csharp
static readonly string TempFolder = System.IO.Path.GetTempPath();
```

---

#### 📍 اعلان چند فیلد همزمان (Declaring Multiple Fields Together)

برای راحتی، می‌توانید چندین فیلد از یک نوع را در یک لیست جداشده با کاما تعریف کنید.
این روش باعث می‌شود که همه فیلدها **attributes** و **field modifiers** یکسانی داشته باشند:

```csharp
static readonly int legs = 8,
                   eyes = 2;
```

### 🌟 ثابت‌ها (Constants)

یک **constant** در زمان **compile time** به‌صورت ایستا (statically) ارزیابی می‌شود و کامپایلر مقدار آن را در هر جایی که استفاده شود، جایگزین می‌کند (شبیه به **macro** در ++C).

یک **constant** می‌تواند از نوع‌های زیر باشد:

* `bool`
* `char`
* `string`
* هر یک از نوع‌های عددی **built-in**
* یا یک **enum type**

---

#### 📝 تعریف constant

یک **constant** با استفاده از کلمه کلیدی **`const`** تعریف می‌شود و باید در هنگام اعلان، مقداردهی اولیه شود.

مثال:

```csharp
public class Test
{
    public const string Message = "Hello World";
}
```

---

#### ⚖️ تفاوت با `static readonly`

یک constant می‌تواند نقشی شبیه به یک **static readonly field** ایفا کند، اما بسیار **محدودتر** است؛ هم از نظر نوع داده‌هایی که قابل استفاده‌اند و هم از نظر قواعد مقداردهی اولیه.

همچنین، تفاوت مهم این است که **constant در زمان compile** ارزیابی می‌شود، در حالی که مقدار یک **static readonly** ممکن است در زمان اجرا تعیین شود.

برای مثال:

```csharp
public static double Circumference(double radius)
{
    return 2 * System.Math.PI * radius;
}
```

در زمان کامپایل به این تبدیل می‌شود:

```csharp
public static double Circumference(double radius)
{
    return 6.2831853071795862 * radius;
}
```

🔹 بنابراین منطقی است که `PI` به‌صورت یک **constant** تعریف شود چون مقدار آن از قبل مشخص است.

در مقابل:

```csharp
static readonly DateTime StartupTime = DateTime.Now;
```

هر بار که برنامه اجرا شود، مقدار متفاوتی خواهد داشت.

---

#### ⚠️ مشکل نسخه‌ها با constant

اگر یک **assembly** (مثلاً X) یک constant را expose کند:

```csharp
public const decimal ProgramVersion = 2.3m;
```

و یک **assembly** دیگر (مثلاً Y) از آن استفاده کند، مقدار `2.3` هنگام کامپایل در assembly Y **ثابت‌سازی (baked in)** می‌شود.
اگر بعدها X با مقدار جدید (مثلاً `2.4`) دوباره کامپایل شود، Y همچنان مقدار قدیمی `2.3` را استفاده می‌کند تا زمانی که Y هم دوباره کامپایل شود.

🔹 استفاده از **static readonly field** این مشکل را برطرف می‌کند.

به بیان دیگر: هر مقداری که **ممکن است در آینده تغییر کند**، ذاتاً **constant** نیست و نباید به‌عنوان constant تعریف شود.

---

#### 📍 ثابت‌های محلی (Local Constants)

شما می‌توانید داخل یک متد هم constant تعریف کنید:

```csharp
void Test()
{
    const double twoPI = 2 * System.Math.PI;
    ...
}
```

---

#### 🔑 modifiers مجاز برای constantهای غیرمحلی

* **Access modifiers** → `public`, `internal`, `private`, `protected`
* **Inheritance modifier** → `new`

---

### 🔧 متدها (Methods)

یک **method** عملی را در قالب یک سری **statement**ها اجرا می‌کند.

* یک متد می‌تواند داده‌های ورودی را از **caller** دریافت کند (با تعریف **parameters**).
* و داده خروجی را به **caller** بازگرداند (با تعریف **return type**).
* اگر یک متد **`void`** باشد، به این معناست که هیچ مقداری را برنمی‌گرداند.
* همچنین می‌تواند با استفاده از **ref/out parameters** داده را به caller بازگرداند.

---

#### 📌 امضای متد (Method Signature)

* امضای یک متد باید در یک type **منحصر‌به‌فرد** باشد.
* امضا شامل **نام متد** و **انواع پارامترها** به‌ترتیب است.
* نام پارامترها و نوع بازگشتی (return type) بخشی از امضا نیستند.

---

#### 🔑 modifiers مجاز برای متدها

* **Static modifier** → `static`
* **Access modifiers** → `public`, `internal`, `private`, `protected`
* **Inheritance modifiers** → `new`, `virtual`, `abstract`, `override`, `sealed`
* **Partial method modifier** → `partial`
* **Unmanaged code modifiers** → `unsafe`, `extern`
* **Asynchronous code modifier** → `async`

---

#### ➡️ متدهای expression-bodied

اگر متدی فقط یک **expression** داشته باشد:

```csharp
int Foo(int x) { return x * 2; }
```

می‌توان آن را به‌شکل کوتاه‌تری نوشت:

```csharp
int Foo(int x) => x * 2;
```

حتی اگر متد **`void`** باشد:

```csharp
void Foo(int x) => Console.WriteLine(x);
```

---

#### 📍 متدهای محلی (Local Methods)

می‌توانید یک متد را داخل متدی دیگر تعریف کنید:

```csharp
void WriteCubes()
{
    Console.WriteLine(Cube(3));
    Console.WriteLine(Cube(4));
    Console.WriteLine(Cube(5));

    int Cube(int value) => value * value * value;
}
```

* متد محلی (`Cube`) فقط در همان متد والد (`WriteCubes`) قابل مشاهده است.
* این کار باعث ساده‌تر شدن type می‌شود و سریعاً نشان می‌دهد که Cube جای دیگری استفاده نمی‌شود.
* متدهای محلی می‌توانند به متغیرها و پارامترهای متد والد دسترسی داشته باشند.

📌 جزئیات بیشتر در بخش **Capturing Outer Variables** (صفحه ۱۹۰) توضیح داده خواهد شد.

* متدهای محلی می‌توانند درون دیگر توابع مانند property accessors، constructorها و غیره نیز تعریف شوند.
* حتی می‌توان متدهای محلی را داخل دیگر متدهای محلی یا داخل **lambda expression**ها (با statement block) قرار داد.
* متدهای محلی می‌توانند **iterator** (فصل ۴) یا **asynchronous** (فصل ۱۴) باشند.

---

#### ⚡ متدهای محلی static (Static Local Methods)

از **C# 8** به بعد، افزودن **static modifier** به یک متد محلی باعث می‌شود آن متد دیگر به متغیرها و پارامترهای متد والد دسترسی نداشته باشد.

🔹 این کار وابستگی (coupling) را کاهش می‌دهد و جلوی استفاده تصادفی از متغیرهای متد والد را می‌گیرد.

### 📍 متدهای محلی و Top-Level Statements

هر **method** که در **top-level statements** تعریف کنید، در واقع به‌عنوان یک **local method** در نظر گرفته می‌شود.
این یعنی (مگر این‌که با `static` علامت‌گذاری شود) می‌توانند به متغیرهای تعریف‌شده در top-level دسترسی داشته باشند:

```csharp
int x = 3;
Foo();

void Foo() => Console.WriteLine(x);
```

---

### 🔄 Overloading متدها

* **Local methods** قابل **overload** شدن نیستند.
  بنابراین، متدهایی که در **top-level statements** تعریف می‌شوند (که در واقع همان local methods هستند)، نمی‌توانند overload شوند.

* اما یک **type** می‌تواند متدها را overload کند؛ یعنی چند متد با نام یکسان داشته باشد، به‌شرطی که **امضای آن‌ها (signature)** متفاوت باشد.

مثال:

```csharp
void Foo(int x) {...}
void Foo(double x) {...}
void Foo(int x, float y) {...}
void Foo(float x, int y) {...}
```

📌 اما مثال‌های زیر **خطای زمان کامپایل** دارند، چون `return type` و `params modifier` بخشی از **امضا** محسوب نمی‌شوند:

```csharp
void  Foo(int x) {...}
float Foo(int x) {...}           // Compile-time error

void  Goo(int[] x) {...}
void  Goo(params int[] x) {...}  // Compile-time error
```

---

#### 📌 پارامترهای ref و out در امضا

این‌که پارامترها به‌صورت **by-value** یا **by-reference** پاس داده شوند، بخشی از امضا محسوب می‌شود.

✅ بنابراین:

```csharp
void Foo(int x) {...}
void Foo(ref int x) {...}   // OK
```

❌ اما:

```csharp
void Foo(out int x) {...}   // Compile-time error
```

زیرا `ref` و `out` به‌طور هم‌زمان نمی‌توانند overload شوند.

---

### 🏗️ Instance Constructors

**Constructor**ها کدی برای مقداردهی اولیه (initialization) روی یک **class** یا **struct** اجرا می‌کنند.
یک constructor مانند یک متد تعریف می‌شود، با این تفاوت که **نام متد** همان **نام type والد** است و هیچ **return type** ندارد:

```csharp
Panda p = new Panda("Petey");   // Call constructor

public class Panda
{
    string name;                // Define field

    public Panda(string n)      // Define constructor
    {
        name = n;               // Initialization code (set up field)
    }
}
```

---

#### 🔑 modifiers مجاز برای constructors

* **Access modifiers** → `public`, `internal`, `private`, `protected`
* **Unmanaged code modifiers** → `unsafe`, `extern`

---

#### ➡️ Expression-bodied constructors

اگر constructor یک statement ساده داشته باشد، می‌تواند به‌شکل کوتاه نوشته شود:

```csharp
public Panda(string n) => name = n;
```

---

#### 📌 رفع ابهام با `this`

اگر نام پارامتر با نام فیلد یکی باشد، می‌توانید با **`this`** فیلد را مشخص کنید:

```csharp
public Panda(string name) => this.name = name;
```

---

### 🔄 Overloading Constructors

یک کلاس یا struct می‌تواند **constructorهای overload** داشته باشد.

برای جلوگیری از تکرار کد، یک constructor می‌تواند constructor دیگری را با استفاده از **`this`** صدا بزند:

```csharp
public class Wine
{
    public decimal Price;
    public int Year;

    public Wine(decimal price) => Price = price;

    public Wine(decimal price, int year) : this(price) => Year = year;
}
```

📌 در این حالت، constructor فراخوانی‌شده **ابتدا** اجرا می‌شود.

می‌توانید یک **عبارت** را نیز به constructor دیگر پاس دهید:

```csharp
public Wine(decimal price, DateTime year) : this(price, year.Year) { }
```

⚠️ در این مرحله، فقط می‌توان به **اعضای static کلاس** دسترسی داشت، نه اعضای instance؛ چون هنوز شیء توسط constructor مقداردهی نشده است.

---

#### 🛠️ جایگزین بهتر: پارامتر اختیاری

مثال بالا را می‌توان با یک constructor ساده‌تر نوشت:

```csharp
public Wine(decimal price, int year = 0)
{
    Price = price;
    Year = year;
}
```

📌 راه‌حل دیگری نیز در بخش **Object Initializers** (صفحه ۱۱۱) معرفی خواهد شد.

---

### 🆓 Implicit Parameterless Constructors

برای **class**ها، کامپایلر C# به‌طور خودکار یک **constructor بدون پارامتر public** تولید می‌کند، به‌شرطی که شما هیچ constructor دیگری تعریف نکرده باشید.
اما به محض این‌که حداقل یک constructor تعریف کنید، دیگر این constructor پیش‌فرض ساخته نمی‌شود.

---

### 🔄 ترتیب مقداردهی اولیه فیلد و constructor

همان‌طور که دیدیم، می‌توان فیلدها را هنگام اعلان مقداردهی کرد:

```csharp
class Player
{
    int shields = 50;   // Initialized first
    int health = 100;   // Initialized second
}
```

📌 مقداردهی اولیه فیلدها **قبل از اجرای constructor** انجام می‌شود و به‌ترتیب اعلان فیلدها اجرا می‌گردد.

### 🔹 سازنده‌های غیرعمومی (Nonpublic constructors)

سازنده‌ها الزامی ندارند که عمومی (**public**) باشند. یک دلیل رایج برای داشتن سازنده‌های غیرعمومی این است که ایجاد نمونه‌ها (**instance creation**) فقط از طریق یک متد ایستا (**static method**) کنترل شود.

به‌عنوان مثال، متد ایستا می‌تواند به‌جای ایجاد یک شیء جدید، یک شیء موجود را از **object pool** برگرداند، یا بر اساس ورودی‌های مختلف، زیرکلاس‌های متفاوتی را بازگرداند:

```csharp
public class Class1
{
    Class1() {}   // سازنده خصوصی

    public static Class1 Create (...)
    {
        // اجرای منطق سفارشی برای برگرداندن یک شیء از نوع Class1
        ...
    }
}
```

---

### 🔹 دیکانستراکتور (Deconstructor)

**دیکانستراکتور** یا **متد deconstructing** تقریباً برعکس یک سازنده عمل می‌کند:

* سازنده معمولاً مجموعه‌ای از مقادیر ورودی (پارامترها) را گرفته و آن‌ها را به فیلدها نسبت می‌دهد.
* دیکانستراکتور برعکس این کار را انجام می‌دهد: فیلدها را به مجموعه‌ای از متغیرها برمی‌گرداند.

🔸 قوانین:

* نام این متد باید دقیقاً `Deconstruct` باشد.
* باید دارای یک یا چند **پارامتر out** باشد.

مثال:

```csharp
class Rectangle
{
    public readonly float Width, Height;

    public Rectangle (float width, float height)
    {
        Width = width;
        Height = height;
    }

    public void Deconstruct (out float width, out float height)
    {
        width = Width;
        height = Height;
    }
}
```

---

### 🔹 فراخوانی دیکانستراکتور

```csharp
var rect = new Rectangle(3, 4);
(float width, float height) = rect;   // دیکانستراکشن
Console.WriteLine(width + " " + height);   // 3 4
```

🔹 این فراخوانی معادل است با:

```csharp
float width, height;
rect.Deconstruct(out width, out height);
```

یا:

```csharp
rect.Deconstruct(out var width, out var height);
```

---

### 🔹 نکات مهم در دیکانستراکشن

* امکان استفاده از **implicit typing** وجود دارد:

  ```csharp
  (var width, var height) = rect;
  var (width, height) = rect;
  ```
* اگر به یکی از متغیرها نیازی ندارید، می‌توانید از نماد **discard (`_`)** استفاده کنید:

  ```csharp
  var (_, height) = rect;
  ```
* اگر متغیرها قبلاً تعریف شده باشند، می‌توانید **نوع‌ها را حذف کنید**:

  ```csharp
  float width, height;
  (width, height) = rect;   // دیکانستراکشن assignment
  ```
* می‌توانید از دیکانستراکشن برای ساده‌سازی سازنده‌ها استفاده کنید:

  ```csharp
  public Rectangle (float width, float height) =>
      (Width, Height) = (width, height);
  ```
* امکان **overload کردن متد Deconstruct** برای ارائه گزینه‌های بیشتر وجود دارد.
* متد `Deconstruct` می‌تواند یک **extension method** هم باشد (برای دیکانستراکشن روی تایپ‌هایی که نویسنده‌ی آن نیستید).
* از C# 10 به بعد، می‌توانید **متغیرهای موجود و جدید** را با هم ترکیب کنید:

  ```csharp
  double x1 = 0;
  (x1, double y2) = rect;
  ```

---

### 🔹 مقداردهی اولیه شیء (Object Initializers)

برای ساده‌سازی مقداردهی اولیه، می‌توان **فیلدها یا propertyهای قابل دسترسی** را مستقیماً بعد از سازنده تنظیم کرد.

مثال کلاس:

```csharp
public class Bunny
{
    public string Name;
    public bool LikesCarrots, LikesHumans;

    public Bunny() {}
    public Bunny(string n) => Name = n;
}
```

📌 نمونه‌سازی با مقداردهی اولیه شیء:

```csharp
// سازنده بدون پارامتر می‌تواند پرانتز خالی را حذف کند
Bunny b1 = new Bunny { Name="Bo", LikesCarrots=true, LikesHumans=false };
Bunny b2 = new Bunny("Bo") { LikesCarrots=true, LikesHumans=false };
```

این کد دقیقاً معادل موارد زیر است (کامپایلر متغیرهای موقت می‌سازد):

```csharp
Bunny temp1 = new Bunny();
temp1.Name = "Bo";
temp1.LikesCarrots = true;
temp1.LikesHumans = false;
Bunny b1 = temp1;

Bunny temp2 = new Bunny("Bo");
temp2.LikesCarrots = true;
temp2.LikesHumans = false;
Bunny b2 = temp2;
```

🔸 دلیل استفاده از متغیرهای موقت این است که اگر در طول مقداردهی اولیه **exception** رخ دهد، شیء **نیمه‌سازمان‌یافته (half-initialized)** باقی نماند.

### 🔹 مقایسه مقداردهی اولیه شیء با پارامترهای اختیاری (Object Initializers Versus Optional Parameters)

به جای استفاده از **object initializer**، می‌توانیم سازنده‌ی کلاس `Bunny` را طوری بنویسیم که یک پارامتر اجباری و دو پارامتر اختیاری داشته باشد:

```csharp
public Bunny (string name,
              bool likesCarrots = false,
              bool likesHumans = false)
{
    Name = name;
    LikesCarrots = likesCarrots;
    LikesHumans = likesHumans;
}
```

📌 این کار به ما اجازه می‌دهد شیء را به شکل زیر بسازیم:

```csharp
Bunny b1 = new Bunny(name: "Bo", likesCarrots: true);
```

---

#### ✅ مزیت تاریخی استفاده از سازنده‌ها

* در گذشته استفاده از سازنده‌ها برای مقداردهی اولیه مزیت داشت، چون می‌توانستیم فیلدها یا propertyها را **فقط-خواندنی (read-only)** کنیم.
* ایجاد فیلد یا property فقط‌خواندنی کار خوبی است وقتی که نیازی به تغییر مقدار آن در طول عمر شیء وجود ندارد.
* اما از **C# 9** به بعد، با **init modifier** می‌توانیم همین هدف را با **object initializer** نیز محقق کنیم.

---

#### ❌ معایب پارامترهای اختیاری

1. **نداشتن پشتیبانی آسان از تغییرات غیرمخرب (nondestructive mutation)**

   * سازنده با پارامترهای اختیاری اجازه‌ی تغییر بدون تخریب (immutable-friendly mutation) را به‌سادگی نمی‌دهد.
   * این مشکل در بخش **Records** (صفحه 227) حل خواهد شد.

2. **مشکل در backward compatibility (سازگاری عقب‌رو)**

   * وقتی از پارامترهای اختیاری در کتابخانه‌های عمومی استفاده شود، افزودن پارامتر اختیاری جدید در آینده باعث می‌شود **سازگاری دودویی (binary compatibility)** با مصرف‌کنندگان قبلی شکسته شود.
   * به‌خصوص در کتابخانه‌هایی که روی **NuGet** منتشر می‌شوند مشکل‌ساز است.

🔹 دلیل: مقدار پارامترهای اختیاری در **کد فراخوانی‌کننده** کامپایل و جایگزین می‌شود.
مثال:

```csharp
Bunny b1 = new Bunny("Bo", true, false);
```

اگر بعداً پارامتر جدیدی مانند `likesCats` اضافه شود، اسمبلی مصرف‌کننده که دوباره کامپایل نشده همچنان متدی با سه پارامتر را صدا می‌زند که دیگر وجود ندارد → خطا در زمان اجرا.

حتی تغییر مقدار پیش‌فرض یکی از پارامترهای اختیاری نیز مشکل ایجاد می‌کند: مصرف‌کنندگان قدیمی تا زمان بازکامپایل از مقدار قدیمی استفاده می‌کنند.

---

#### 🔹 ملاحظه دیگر

سازنده‌ها روی **ارث‌بری (Inheritance)** هم اثر می‌گذارند (بحث در صفحه 126).

* داشتن چند سازنده با لیست‌های طولانی پارامتر، زیرکلاس‌سازی را سخت می‌کند.
* راه‌حل: تعداد و پیچیدگی سازنده‌ها را به حداقل برسانید و از **object initializer** برای جزئیات استفاده کنید.

---

### 🔹 مرجع `this`

کلمه‌ی کلیدی `this` به نمونه‌ی جاری (instance) اشاره می‌کند.

مثال:

```csharp
public class Panda
{
    public Panda Mate;
    public void Marry(Panda partner)
    {
        Mate = partner;
        partner.Mate = this;
    }
}
```

همچنین `this` برای رفع ابهام میان فیلد و پارامتر/متغیر محلی استفاده می‌شود:

```csharp
public class Test
{
    string name;
    public Test(string name) => this.name = name;
}
```

📌 مرجع `this` فقط در اعضای **nonstatic** کلاس یا struct معتبر است.

---

### 🔹 ویژگی‌ها (Properties)

ویژگی‌ها (**Properties**) از بیرون شبیه فیلدها به نظر می‌رسند، اما در درون منطق دارند (مثل متدها).

مثال:

```csharp
Stock msft = new Stock();
msft.CurrentPrice = 30;
msft.CurrentPrice -= 3;
Console.WriteLine(msft.CurrentPrice);
```

در ظاهر معلوم نیست که `CurrentPrice` یک فیلد است یا property.

🔸 تعریف property:

```csharp
public class Stock
{
    decimal currentPrice;    // فیلد خصوصی (backing field)
    public decimal CurrentPrice   // property عمومی
    {
        get { return currentPrice; }
        set { currentPrice = value; }
    }
}
```

* **get accessor** وقتی property خوانده می‌شود اجرا می‌شود.
* **set accessor** وقتی مقداردهی می‌شود اجرا می‌شود و پارامتر ضمنی `value` دارد.

📌 تفاوت اصلی با فیلدها:

* property امکان کنترل کامل روی خواندن/نوشتن مقدار را به برنامه‌نویس می‌دهد.
* مثلاً می‌توان در setter بررسی کرد که مقدار ورودی در بازه معتبر باشد، و در غیر این صورت خطا پرتاب کرد.

---

### 🔹 اصلاح‌کننده‌های مجاز روی Properties

* **Static modifier:** `static`
* **Access modifiers:** `public internal private protected`
* **Inheritance modifiers:** `new virtual abstract override sealed`
* **Unmanaged code modifiers:** `unsafe extern`

---

### 🔹 ویژگی‌های فقط‌خواندنی و محاسباتی (Read-only and calculated properties)

* اگر فقط **get accessor** تعریف شود → property فقط‌خواندنی است.
* اگر فقط **set accessor** تعریف شود → property فقط‌نوشتنی است (به ندرت استفاده می‌شود).
* property می‌تواند از داده‌های دیگر محاسبه شود:

```csharp
decimal currentPrice, sharesOwned;
public decimal Worth
{
    get { return currentPrice * sharesOwned; }
}
```

---

### 🔹 ویژگی‌های Expression-Bodied

برای نوشتن کوتاه‌تر propertyهای فقط‌خواندنی:

```csharp
public decimal Worth => currentPrice * sharesOwned;
```

حتی setter هم می‌تواند expression-bodied باشد:

```csharp
public decimal Worth
{
    get => currentPrice * sharesOwned;
    set => sharesOwned = value / currentPrice;
}
```

---

### 🔹 ویژگی‌های خودکار (Automatic Properties)

رایج‌ترین نوع property، ساده‌ترین پیاده‌سازی است: فقط خواندن/نوشتن به یک فیلد خصوصی.

به جای نوشتن دستی، می‌توان از property خودکار استفاده کرد:

```csharp
public class Stock
{
    public decimal CurrentPrice { get; set; }
}
```

* کامپایلر به طور خودکار یک **backing field خصوصی** تولید می‌کند.
* می‌توان setter را **private** یا **protected** کرد تا property برای دیگران فقط‌خواندنی باشد.

📌 ویژگی‌های خودکار از C# 3.0 معرفی شدند.

---

### 🔹 مقداردهی اولیه property (Property Initializers)

propertyهای خودکار می‌توانند مستقیماً مقدار اولیه داشته باشند:

```csharp
public decimal CurrentPrice { get; set; } = 123;
```

* اینجا مقدار اولیه‌ی `CurrentPrice` برابر با 123 است.
* property با initializer می‌تواند **فقط‌خواندنی** هم باشد:

```csharp
public int Maximum { get; } = 999;
```

مانند فیلدهای readonly، propertyهای خودکار readonly نیز می‌توانند در **constructor** مقداردهی شوند.
این قابلیت برای ساختن **انواع immutable (فقط‌خواندنی)** بسیار کاربردی است.

### 🔹 دسترسی در get و set (get and set accessibility)

در C# می‌توان **سطح دسترسی** (accessibility) متدهای `get` و `set` یک property را متفاوت تعریف کرد.

📌 کاربرد رایج: داشتن یک property عمومی (**public**) با یک setter خصوصی یا داخلی (**private/internal**).

مثال:

```csharp
public class Foo
{
    private decimal x;
    public decimal X
    {
        get         { return x; }
        private set { x = Math.Round(value, 2); }
    }
}
```

🔑 دقت کنید: property خودش سطح دسترسی بازتر (اینجا `public`) دارد، اما setter سطح دسترسی محدودتر (`private`).

---

### 🔹 Setterهای فقط-init (Init-only setters)

از **C# 9** می‌توان به جای `set` از **init** استفاده کرد:

```csharp
public class Note
{
    public int Pitch    { get; init; } = 20;   // property فقط-init
    public int Duration { get; init; } = 100;  // property فقط-init
}
```

* این propertyها مانند read-only عمل می‌کنند، اما می‌توانند با **object initializer** مقداردهی شوند:

```csharp
var note = new Note { Pitch = 50 };
```

* بعد از آن دیگر نمی‌توان مقدار را تغییر داد:

```csharp
note.Pitch = 200;   // ❌ خطا – setter فقط-init
```

* حتی از داخل کلاس هم قابل تغییر نیستند (مگر در initializer، constructor یا یک accessor دیگر از نوع init).

---

### 🔹 جایگزین init-only properties

راه جایگزین، تعریف propertyهای فقط‌خواندنی و مقداردهی آن‌ها در constructor است:

```csharp
public class Note
{
    public int Pitch    { get; }
    public int Duration { get; }
    public Note (int pitch = 20, int duration = 100)
    {
        Pitch = pitch; Duration = duration;
    }
}
```

❗ مشکل: اگر این کلاس در یک **کتابخانه عمومی** باشد، افزودن پارامتر اختیاری جدید به constructor باعث شکستن **سازگاری دودویی (binary compatibility)** می‌شود.
اما افزودن یک **property فقط-init** هیچ مشکلی ایجاد نمی‌کند.

---

### 🔹 مزایای init-only properties

* پشتیبانی از **nondestructive mutation** وقتی با **records** استفاده شوند (بخش Records، صفحه 227).
* امکان پیاده‌سازی setter با منطق داخلی:

```csharp
public class Note
{
    readonly int _pitch;
    public int Pitch { get => _pitch; init => _pitch = value; }
}
```

💡 نکته: حتی می‌توان فیلدهای **readonly** را در setter فقط-init تغییر داد. این باعث می‌شود کلاس **immutable داخلی** باقی بماند.

⚠ تغییر یک accessor از `init` به `set` (یا برعکس) یک **تغییر سازگاری‌شکن (binary breaking change)** است. بنابراین اسمبلی‌های دیگر باید دوباره کامپایل شوند.

---

### 🔹 پیاده‌سازی property در CLR

در سطح CLR، propertyها به متدهایی تبدیل می‌شوند:

```csharp
public decimal get_CurrentPrice {...}
public void set_CurrentPrice (decimal value) {...}
```

* یک accessor از نوع `init` مثل `set` پردازش می‌شود، اما یک flag اضافه در متادیتا دارد.
* propertyهای ساده و غیرمجازی توسط **JIT compiler** inline می‌شوند (یعنی فراخوانی متد با بدنه‌اش جایگزین می‌شود).
* در نتیجه، دسترسی به property و فیلد از نظر کارایی **هیچ تفاوتی ندارد**.

---

### 🔹 ایندکسرها (Indexers)

ایندکسرها语 🌟 اجازه می‌دهند یک کلاس یا struct مثل آرایه رفتار کند.

📌 `string` یک ایندکسر دارد که اجازه می‌دهد با استفاده از یک اندیس عددی به هر `char` دسترسی پیدا کنید:

```csharp
string s = "hello";
Console.WriteLine(s[0]);  // h
Console.WriteLine(s[3]);  // l
```

* ایندکسرها مثل property هستند، اما به جای نام، با **اندیس (index argument)** صدا زده می‌شوند.
* اندیس می‌تواند از هر نوعی باشد، نه فقط عدد صحیح.
* همان modifierهای property را دارند (`public, private, static, ...`).
* می‌توانند به صورت **null-شرطی** استفاده شوند:

```csharp
string s = null;
Console.WriteLine(s?[0]);  // چیزی چاپ نمی‌شود، خطایی هم رخ نمی‌دهد
```

---

### 🔹 پیاده‌سازی ایندکسر

برای تعریف ایندکسر، یک property با نام `this` می‌سازیم و پارامترها را داخل کروشه قرار می‌دهیم:

```csharp
class Sentence
{
    string[] words = "The quick brown fox".Split();
    public string this[int wordNum]   // ایندکسر
    {
        get { return words[wordNum]; }
        set { words[wordNum] = value; }
    }
}
```

مثال استفاده:

```csharp
Sentence s = new Sentence();
Console.WriteLine(s[3]);   // fox
s[3] = "kangaroo";
Console.WriteLine(s[3]);   // kangaroo
```

* یک کلاس می‌تواند چندین ایندکسر با نوع پارامترهای متفاوت داشته باشد.
* ایندکسر می‌تواند بیش از یک پارامتر بگیرد:

```csharp
public string this[int arg1, string arg2]
{
    get { ... }
    set { ... }
}
```

* اگر setter حذف شود → ایندکسر فقط‌خواندنی می‌شود.
* می‌توان با **expression-bodied** نوشت:

```csharp
public string this[int wordNum] => words[wordNum];
```

---

### 🔹 پیاده‌سازی ایندکسر در CLR

ایندکسرها به متدهای زیر کامپایل می‌شوند:

```csharp
public string get_Item(int wordNum) {...}
public void set_Item(int wordNum, string value) {...}
```

---

### 🔹 استفاده از Indices و Ranges در ایندکسرها

می‌توان ایندکسرهایی برای `Index` و `Range` تعریف کرد (بخش Indices and Ranges، صفحه 63).

مثال توسعه کلاس Sentence:

```csharp
public string this[Index index] => words[index];
public string[] this[Range range] => words[range];
```

مثال استفاده:

```csharp
Sentence s = new Sentence();
Console.WriteLine(s[^1]);          // fox
string[] firstTwoWords = s[..2];   // (The, quick)
```

### 🔹 سازنده‌های اصلی (Primary Constructors) در #C 12

از نسخه‌ی **#C 12** می‌توانید یک لیست پارامتر را مستقیماً بعد از تعریف یک کلاس (یا struct) قرار دهید:

```csharp
class Person (string firstName, string lastName)
{
  public void Print() => Console.WriteLine(firstName + " " + lastName);
}
```

این کار به کامپایلر دستور می‌دهد که به‌طور خودکار یک **سازنده‌ی اصلی (primary constructor)** با استفاده از پارامترهای `firstName` و `lastName` بسازد. در نتیجه می‌توانیم کلاس خود را به این شکل نمونه‌سازی کنیم:

```csharp
Person p = new Person("Alice", "Jones");
p.Print();    // Alice Jones
```

⚡ سازنده‌های اصلی برای **نمونه‌سازی سریع (prototyping)** و سناریوهای ساده مفید هستند.
روش جایگزین این است که فیلدها را تعریف کرده و یک سازنده معمولی بنویسید:

```csharp
class Person    // (بدون primary constructor)
{
  string firstName, lastName;   // تعریف فیلدها

  public Person(string firstName, string lastName)   // سازنده
  {
    this.firstName = firstName;   // انتساب به فیلد
    this.lastName = lastName;     // انتساب به فیلد
  }

  public void Print() => Console.WriteLine(firstName + " " + lastName);
}
```

سازنده‌ای که توسط #C ساخته می‌شود، **اصلی (primary)** نامیده می‌شود، زیرا هر سازنده دیگری که خودتان به‌طور صریح تعریف کنید، **باید آن را فراخوانی کند**:

```csharp
class Person (string firstName, string lastName)
{
  public Person(string firstName, string lastName, int age)
    : this(firstName, lastName)   // باید primary constructor فراخوانی شود
  {
  }
  ...
}
```

این باعث می‌شود پارامترهای سازنده‌ی اصلی همیشه مقداردهی شوند ✅.

---

### 📝 نکته درباره‌ی **Records**

در #C علاوه بر کلاس‌ها، **Records** هم وجود دارند (در صفحه‌ی 227 توضیح داده شده است).
Records نیز از **primary constructor** پشتیبانی می‌کنند، اما کامپایلر یک مرحله‌ی اضافه انجام می‌دهد: برای هر پارامتر سازنده‌ی اصلی، به‌طور پیش‌فرض یک **property عمومی با init-only** تولید می‌کند.
اگر چنین رفتاری مدنظر شماست، بهتر است از **Record** استفاده کنید.

---

### ⚠️ محدودیت‌های Primary Constructor

این سازنده‌ها بیشتر برای سناریوهای ساده مناسب‌اند، زیرا:

1. نمی‌توانید کد اضافی برای مقداردهی اولیه داخل primary constructor اضافه کنید.
2. اگر بخواهید پارامترها را به‌عنوان property عمومی منتشر کنید، اضافه کردن **منطق اعتبارسنجی (validation logic)** آسان نیست (مگر property فقط خواندنی باشد).

🔸 همچنین سازنده‌ی اصلی جایگزین **سازنده‌ی پیش‌فرض بدون پارامتر** می‌شود که در حالت عادی #C تولید می‌کرد.

---

### 📌 رفتار سازنده‌ی اصلی در مقایسه با سازنده معمولی

در یک سازنده معمولی:

```csharp
class Person
{
  public Person(string firstName, string lastName)
  {
    // انجام عملیات با firstName, lastName
  }
}
```

وقتی کد داخل سازنده تمام شد، پارامترها (`firstName` و `lastName`) از محدوده‌ی دید خارج می‌شوند و دیگر قابل دسترسی نیستند.

اما در **primary constructor**، پارامترها از محدوده خارج نمی‌شوند و می‌توان آن‌ها را در کل بدنه‌ی کلاس و طول عمر شیء استفاده کرد.

---

### ⚙️ فیلدها و Property‌ها در Primary Constructor

دسترسی پارامترها به **initializerها** نیز گسترش می‌یابد:

```csharp
class Person (string firstName, string lastName)
{
  public readonly string FirstName = firstName;  // فیلد
  public string LastName { get; } = lastName;    // Property
}
```

---

### 🎭 ماسک‌گذاری (Masking) پارامترها

فیلدها یا Propertyها می‌توانند همان نام پارامترها را بگیرند:

```csharp
class Person (string firstName, string lastName)
{
  readonly string firstName = firstName;
  readonly string lastName = lastName;

  public void Print() => Console.WriteLine(firstName + " " + lastName);
}
```

در این حالت، فیلد یا property بر پارامتر **اولویت دارد** (پارامتر را mask می‌کند)، مگر در سمت راست initializerها.

---

### ✨ اعتبارسنجی و پردازش مقادیر

مثال ذخیره‌سازی FullName با محاسبه در initializer:

```csharp
new Person("Alice", "Jones").Print();   // Alice Jones

class Person (string firstName, string lastName)
{
  public readonly string FullName = firstName + " " + lastName;
  public void Print() => Console.WriteLine(FullName);
}
```

مثال ذخیره‌ی مقدار uppercase برای `lastName`:

```csharp
new Person("Alice", "Jones").Print();   // Alice JONES

class Person (string firstName, string lastName)
{
  readonly string lastName = lastName.ToUpper();
  public void Print() => Console.WriteLine(firstName + " " + lastName);
}
```

---

### 🚨 اعتبارسنجی با استثنا (Exception)

می‌توان از **throw expression** در initializer استفاده کرد:

```csharp
new Person("Alice", null);   // ArgumentNullException

class Person (string firstName, string lastName)
{
  readonly string lastName = (lastName == null)
    ? throw new ArgumentNullException("lastName")
    : lastName;
}
```

(به یاد داشته باشید: initializerها هنگام ساخته شدن شیء اجرا می‌شوند، نه هنگام دسترسی به فیلد یا property.)

---

### 🔄 انتشار پارامتر به‌عنوان Property با set

```csharp
class Person (string firstName, string lastName)
{
  public string LastName { get; set; } = lastName;
}
```

اما در این حالت اعتبارسنجی ساده نیست، چون باید هم در **set accessor** و هم در **initializer** اعتبارسنجی را پیاده‌سازی کنید (همین مشکل برای init-only هم وجود دارد).
در چنین شرایطی بهتر است از **سازنده‌های معمولی و فیلدهای پشتیبان (backing fields)** استفاده کنید.

### ⚡ سازنده‌های استاتیک (Static Constructors)

🔹 یک **static constructor** فقط یک بار برای هر **نوع (type)** اجرا می‌شود (نه برای هر نمونه).
یک نوع فقط می‌تواند **یک سازنده‌ی استاتیک** داشته باشد، این سازنده باید بدون پارامتر باشد و نام آن دقیقاً همان نام کلاس باشد:

```csharp
class Test
{
  static Test() { Console.WriteLine("Type Initialized"); }
}
```

زمان اجرا (runtime) این سازنده‌ی استاتیک را **به‌طور خودکار درست قبل از اولین استفاده از نوع** فراخوانی می‌کند. دو چیز این سازنده را فعال می‌کند:

1. نمونه‌سازی از نوع 🆕
2. دسترسی به یک عضو استاتیک در آن نوع ⚙️

🔸 تنها **modifier**هایی که در سازنده‌ی استاتیک مجاز هستند عبارت‌اند از: `unsafe` و `extern`.

🚨 اگر یک سازنده‌ی استاتیک استثنای مدیریت‌نشده (unhandled exception) پرتاب کند، آن **نوع تا پایان عمر برنامه غیرقابل‌استفاده می‌شود**.

---

### 🏗️ Module Initializers (از #C 9)

از نسخه‌ی **#C 9** می‌توانید **Module Initializer** تعریف کنید که یک بار برای هر **assembly** و هنگام بارگذاری آن اجرا می‌شود:

```csharp
[System.Runtime.CompilerServices.ModuleInitializer]
internal static void InitAssembly()
{
  ...
}
```

---

### 📌 سازنده‌های استاتیک و ترتیب مقداردهی فیلدها

* مقداردهی اولیه‌ی فیلدهای استاتیک دقیقاً قبل از فراخوانی سازنده‌ی استاتیک اجرا می‌شود.
* اگر نوعی سازنده‌ی استاتیک نداشته باشد، مقداردهی اولیه‌ی فیلدها درست قبل از اولین استفاده از نوع (یا زودتر، بسته به تصمیم runtime) انجام می‌شود.
* مقداردهی اولیه‌ی فیلدها به ترتیبی است که تعریف شده‌اند.

مثال:

```csharp
class Foo
{
  public static int X = Y;   // 0
  public static int Y = 3;   // 3
}
```

اگر جای این دو خط را عوض کنیم، هر دو برابر 3 می‌شوند.

---

مثال دیگر:

```csharp
Console.WriteLine(Foo.X);    // 3

class Foo
{
  public static Foo Instance = new Foo();
  public static int X = 3;

  Foo() => Console.WriteLine(X);   // 0
}
```

در اینجا ابتدا **0** و سپس **3** چاپ می‌شود.
اگر جای دو خط پررنگ را عوض کنیم، خروجی **3 و 3** خواهد بود.

---

### 🏷️ کلاس‌های استاتیک (Static Classes)

* کلاسی که با `static` علامت‌گذاری شده باشد:

  * نمی‌توان از آن نمونه ساخت 🚫
  * نمی‌تواند زیرکلاس شود 🚫
  * باید فقط شامل **اعضای استاتیک** باشد.

📖 نمونه‌های معروف: `System.Console` و `System.Math`.

---

### 🗑️ Finalizers

**Finalizer** متدی است که درست قبل از آزاد شدن حافظه‌ی یک شیء توسط **Garbage Collector** اجرا می‌شود.

سینتکس: نام کلاس همراه با `~`

```csharp
class Class1
{
  ~Class1()
  {
    ...
  }
}
```

این در واقع معادل بازنویسی متد `Finalize` از کلاس `Object` است:

```csharp
protected override void Finalize()
{
  ...
  base.Finalize();
}
```

📌 توضیحات کامل درباره‌ی garbage collection و finalizerها در فصل 12 خواهد آمد.

می‌توانید finalizer تک‌خطی هم بنویسید:

```csharp
~Class1() => Console.WriteLine("Finalizing");
```

---

### 🧩 Partial Types و Partial Methods

🔹 **Partial types** به شما اجازه می‌دهند تعریف یک نوع را به چند بخش (معمولاً در فایل‌های مختلف) تقسیم کنید.

سناریوی رایج:

* یک فایل auto-generated (مثلاً توسط Visual Studio Designer)
* یک فایل دستی برای افزودن کد سفارشی

```csharp
// PaymentFormGen.cs - auto-generated
partial class PaymentForm { ... }

// PaymentForm.cs - hand-authored
partial class PaymentForm { ... }
```

⚠️ هر بخش باید `partial` باشد. این کد غیرقانونی است:

```csharp
partial class PaymentForm {}
class PaymentForm {}
```

* اعضای تکراری (مثلاً سازنده با همان پارامترها) مجاز نیستند.
* **partial types** کاملاً در زمان کامپایل توسط کامپایلر ترکیب می‌شوند.
* همه‌ی بخش‌ها باید در **یک assembly** باشند.
* می‌توانید در برخی بخش‌ها base class مشخص کنید، به شرطی که یکی باشند.
* هر بخش می‌تواند به‌طور مستقل interfaceها را پیاده‌سازی کند.

---

### 🔧 Partial Methods

یک **partial type** می‌تواند شامل **partial method** باشد. این‌ها معمولاً برای فراهم کردن hook در کد auto-generated استفاده می‌شوند:

```csharp
partial class PaymentForm    // فایل auto-generated
{
  partial void ValidatePayment(decimal amount);
}

partial class PaymentForm    // فایل دستی
{
  partial void ValidatePayment(decimal amount)
  {
    if (amount > 100)
      ...
  }
}
```

* یک partial method شامل **تعریف (definition)** و **پیاده‌سازی (implementation)** است.
* تعریف معمولاً توسط generator نوشته می‌شود؛ پیاده‌سازی دستی اضافه می‌شود.
* اگر پیاده‌سازی ارائه نشود، هم تعریف و هم تمام فراخوانی‌های آن حذف می‌شوند ➝ هیچ هزینه‌ای برای کد ندارد ✅.
* partial methodها باید `void` باشند و ذاتاً `private` هستند.
* نمی‌توانند `out parameter` داشته باشند.

### 🧩 متدهای partial توسعه‌یافته (Extended Partial Methods)

🔹 متدهای partial توسعه‌یافته (از C# 9 به بعد) برای سناریوی معکوس تولید کد طراحی شده‌اند؛ جایی که یک برنامه‌نویس **hook**‌هایی تعریف می‌کند که یک **code generator** آن‌ها را پیاده‌سازی می‌کند.
یک نمونه از کاربرد این موضوع، **source generators** هستند (قابلیتی در Roslyn) که به شما اجازه می‌دهند اسمبلی‌ای را به کامپایلر بدهید تا به‌صورت خودکار بخش‌هایی از کد شما را تولید کند.

---

```csharp
public partial class Test
{
  public partial void M1();    // متد partial توسعه‌یافته
  private partial void M2();   // متد partial توسعه‌یافته
}
```

📌 اگر یک اعلان متد partial با یک **accessibility modifier** (مثل public یا private) آغاز شود، آن متد به‌عنوان یک متد partial توسعه‌یافته در نظر گرفته می‌شود.

نکات مهم:

* متدهای partial توسعه‌یافته **حتماً باید پیاده‌سازی داشته باشند**؛ اگر پیاده‌سازی نشوند، از بین نمی‌روند.
* در مثال بالا، هم `M1` و هم `M2` باید پیاده‌سازی شوند.
* این متدها می‌توانند **هر نوع مقداری را برگردانند** و همچنین می‌توانند **پارامترهای out** داشته باشند.

مثال:

```csharp
public partial class Test
{
  public partial bool IsValid (string identifier);
  internal partial bool TryParse (string number, out int result);
}
```

---

### 🏷️ عملگر nameof

🔹 عملگر `nameof` نام هر **symbol** (مثل type، member، variable و …) را به‌صورت یک **string** برمی‌گرداند:

```csharp
int count = 123;
string name = nameof (count);   // مقدار name برابر است با "count"
```

📌 مزیت اصلی `nameof` نسبت به نوشتن مستقیم یک رشته این است که از **static type checking** پشتیبانی می‌کند.
ابزارهایی مثل **Visual Studio** این ارجاع را می‌شناسند، بنابراین اگر شما نام symbol را تغییر دهید، تمام ارجاعات آن نیز به‌طور خودکار تغییر خواهند کرد.

مثال برای **field** یا **property**:

```csharp
string name = nameof (StringBuilder.Length);  // خروجی: "Length"
```

اگر بخواهید `"StringBuilder.Length"` را کامل برگردانید:

```csharp
nameof (StringBuilder) + "." + nameof (StringBuilder.Length);
```

---

### 🏛️ وراثت (Inheritance)

🔹 یک کلاس می‌تواند از کلاس دیگری **ارث‌بری (inherit)** کند تا آن را گسترش یا سفارشی‌سازی کند.
با ارث‌بری می‌توان از امکانات یک کلاس استفاده کرد بدون اینکه از صفر همه‌چیز را بسازید.

* یک کلاس فقط می‌تواند از یک کلاس ارث‌بری کند.
* اما خودش می‌تواند توسط چندین کلاس دیگر ارث‌بری شود (تشکیل **class hierarchy**).

مثال:

```csharp
public class Asset
{
  public string Name;
}

public class Stock : Asset     // inherits from Asset
{
  public long SharesOwned;
}

public class House : Asset     // inherits from Asset
{
  public decimal Mortgage;
}
```

نحوه‌ی استفاده:

```csharp
Stock msft = new Stock { Name="MSFT", SharesOwned=1000 };
Console.WriteLine (msft.Name);        // MSFT
Console.WriteLine (msft.SharesOwned); // 1000

House mansion = new House { Name="Mansion", Mortgage=250000 };
Console.WriteLine (mansion.Name);     // Mansion
Console.WriteLine (mansion.Mortgage); // 250000
```

📌 کلاس‌های `Stock` و `House` فیلد `Name` را از کلاس پایه‌ی `Asset` به ارث می‌برند.

* کلاس مشتق‌شده = **subclass**
* کلاس پایه = **superclass**

---

### 🔄 چندریختی (Polymorphism)

🔹 در C# مراجع (**references**) چندریختی هستند.
این یعنی یک متغیر از نوع `x` می‌تواند به یک شیء از نوعی که زیرکلاس `x` است اشاره کند.

مثال:

```csharp
public static void Display (Asset asset)
{
  System.Console.WriteLine (asset.Name);
}

Stock msft    = new Stock { Name="MSFT" };
House mansion = new House { Name="Mansion" };

Display (msft);     // OK
Display (mansion);  // OK
```

📌 دلیلش این است که کلاس‌های مشتق‌شده (`Stock` و `House`) همه‌ی ویژگی‌های کلاس پایه (`Asset`) را دارند.

اما برعکس درست نیست:

```csharp
Display (new Asset());   // خطای زمان کامپایل

public static void Display (House house)
{
  System.Console.WriteLine (house.Mortgage);
}
```

---

### 🎭 Casting و Reference Conversions

یک مرجع (object reference) می‌تواند:

* به‌طور **ضمنی (implicit)** به کلاس پایه **upcast** شود.
* به‌طور **صریح (explicit)** به زیرکلاس **downcast** شود.

📌 Upcast و Downcast بین انواع سازگار مرجع، درواقع یک **reference conversion** انجام می‌دهند:
یعنی یک مرجع جدید ساخته می‌شود که به همان شیء اشاره می‌کند.

* Upcast همیشه موفق است ✅
* Downcast فقط وقتی موفق است که شیء واقعاً از نوع مناسب باشد ⚠️

---

#### 🔼 Upcasting

```csharp
Stock msft = new Stock();
Asset a = msft;     // Upcast
```

در اینجا، هر دو متغیر `a` و `msft` به همان شیء از نوع `Stock` اشاره می‌کنند:

```csharp
Console.WriteLine (a == msft);   // True
Console.WriteLine (a.Name);      // OK
Console.WriteLine (a.SharesOwned); // خطای زمان کامپایل
```

📌 دلیل خطای آخر: متغیر `a` از نوع `Asset` است، بنابراین فقط می‌تواند اعضای `Asset` را ببیند.
برای دسترسی به `SharesOwned` باید شیء را به `Stock` **downcast** کنید.

### 🔽 Downcasting

**Downcast** عملیاتی است که یک مرجع کلاس پایه را به مرجع کلاس مشتق‌شده تبدیل می‌کند:

```csharp
Stock msft = new Stock();
Asset a = msft;        // Upcast
Stock s = (Stock)a;    // Downcast
Console.WriteLine(s.SharesOwned); // بدون خطا
Console.WriteLine(s == a);        // True
Console.WriteLine(s == msft);     // True
```

* مانند **Upcast**، فقط مرجع تغییر می‌کند و شیء اصلی تغییری نمی‌کند.
* **Downcast** نیاز به cast صریح دارد، زیرا ممکن است در زمان اجرا شکست بخورد:

```csharp
House h = new House();
Asset a = h;        // Upcast همیشه موفق
Stock s = (Stock)a; // Downcast شکست می‌خورد، چون a از نوع Stock نیست
```

⚠️ اگر Downcast شکست بخورد، یک **InvalidCastException** پرتاب می‌شود.

---

### 🔹 عملگر `as`

* انجام Downcast بدون پرتاب Exception در صورت شکست:

```csharp
Asset a = new Asset();
Stock s = a as Stock; // s برابر null است، بدون Exception
```

* این مفید است وقتی می‌خواهید نتیجه را قبل از استفاده بررسی کنید:

```csharp
if (s != null)
    Console.WriteLine(s.SharesOwned);
```

💡 تفاوت با cast صریح:

* `cast` → اطمینان از نوع دارید، اگر اشتباه باشد Exception پرتاب می‌شود.
* `as` → نوع مطمئن نیست، می‌توانید بر اساس null تصمیم بگیرید.

⚠️ محدودیت‌ها:

* نمی‌تواند تبدیل سفارشی یا عددی انجام دهد:

```csharp
long x = 3 as long; // خطای کامپایل
```

---

### 🔹 عملگر `is`

* بررسی می‌کند که یک متغیر با نوع یا الگوی مشخص مطابقت دارد:

```csharp
if (a is Stock)
    Console.WriteLine(((Stock)a).SharesOwned);
```

* می‌توان همزمان با معرفی یک متغیر استفاده کرد:

```csharp
if (a is Stock s)
    Console.WriteLine(s.SharesOwned);
```

* این متغیر حتی خارج از شرط نیز در محدوده‌ی قابل دسترسی باقی می‌ماند:

```csharp
if (a is Stock s && s.SharesOwned > 100000)
    Console.WriteLine("Wealthy");
else
    s = new Stock();

Console.WriteLine(s.SharesOwned); // هنوز در محدوده است
```

---

### 🔹 Virtual Function Members

* متد، پراپرتی، ایندکسر و event می‌توانند **virtual** باشند تا کلاس‌های مشتق آن‌ها را **override** کنند:

```csharp
public class Asset
{
    public string Name;
    public virtual decimal Liability => 0; // Expression-bodied property
}

public class Stock : Asset
{
    public long SharesOwned;
}

public class House : Asset
{
    public decimal Mortgage;
    public override decimal Liability => Mortgage;
}
```

```csharp
House mansion = new House { Name="McMansion", Mortgage=250000 };
Asset a = mansion;

Console.WriteLine(mansion.Liability); // 250000
Console.WriteLine(a.Liability);       // 250000
```

⚠️ **توجه:** فراخوانی متد virtual از داخل constructor می‌تواند خطرناک باشد، زیرا کلاس مشتق ممکن است هنوز به طور کامل مقداردهی نشده باشد.

---

### 🔹 Covariant Return Types (C# 9)

* از C# 9 می‌توان متد را override کرد تا نوع بازگشتی **مشتق شده‌تر** داشته باشد:

```csharp
public class Asset
{
    public string Name;
    public virtual Asset Clone() => new Asset { Name = Name };
}

public class House : Asset
{
    public decimal Mortgage;
    public override House Clone() => new House { Name = Name, Mortgage = Mortgage };
}
```

* قبل از C# 9، نوع بازگشتی باید دقیقاً همان نوع پایه می‌بود:

```csharp
public override Asset Clone() => new House { Name = Name, Mortgage = Mortgage };
```

* برای استفاده از ویژگی‌های House، Downcast لازم بود:

```csharp
House mansion1 = new House { Name="McMansion", Mortgage=250000 };
House mansion2 = (House)mansion1.Clone();
```

### 🔹 Abstract Classes and Abstract Members

* یک کلاس **abstract** هیچ‌گاه نمی‌تواند نمونه‌سازی شود؛ تنها زیرکلاس‌های **concrete** می‌توانند نمونه ایجاد کنند.
* کلاس‌های abstract می‌توانند **abstract members** داشته باشند که شبیه virtual هستند، اما پیاده‌سازی پیش‌فرض ندارند. زیرکلاس موظف است آن‌ها را پیاده‌سازی کند مگر اینکه خودش هم abstract باشد.

```csharp
public abstract class Asset
{
    public abstract decimal NetValue { get; }
}

public class Stock : Asset
{
    public long SharesOwned;
    public decimal CurrentPrice;
    public override decimal NetValue => CurrentPrice * SharesOwned;
}
```

---

### 🔹 Hiding Inherited Members

* اگر کلاس پایه و کلاس مشتق **عضوهای هم‌نام** داشته باشند، عضو کلاس مشتق، عضو کلاس پایه را **مخفی می‌کند**.

```csharp
public class A { public int Counter = 1; }
public class B : A { public int Counter = 2; }
```

* کامپایلر هشدار می‌دهد، اما می‌توان با **modifer `new`** به صورت عمدی مخفی‌سازی کرد:

```csharp
public class B : A { public new int Counter = 2; }
```

* `new` در این زمینه فقط هشدار کامپایلر را حذف می‌کند و قصد برنامه‌نویس را مشخص می‌کند.

---

### 🔹 `new` versus `override`

* `override` → جایگزینی یک متد virtual در کلاس پایه
* `new` → مخفی کردن یک عضو هم‌نام بدون override

```csharp
public class BaseClass
{
    public virtual void Foo() { Console.WriteLine("BaseClass.Foo"); }
}

public class Overrider : BaseClass
{
    public override void Foo() { Console.WriteLine("Overrider.Foo"); }
}

public class Hider : BaseClass
{
    public new void Foo() { Console.WriteLine("Hider.Foo"); }
}
```

```csharp
Overrider over = new Overrider();
BaseClass b1 = over;
over.Foo(); // Overrider.Foo
b1.Foo();   // Overrider.Foo

Hider h = new Hider();
BaseClass b2 = h;
h.Foo();    // Hider.Foo
b2.Foo();   // BaseClass.Foo
```

---

### 🔹 Sealing Functions and Classes

* با `sealed` می‌توان **یک متد override را مسدود** کرد تا توسط کلاس‌های مشتق دیگر override نشود:

```csharp
public sealed override decimal Liability { get { return Mortgage; } }
```

* همچنین می‌توان **یک کلاس را sealed** کرد تا امکان subclassing آن نباشد.

⚠️ نمی‌توان یک عضو را در برابر **مخفی شدن با `new`** مسدود کرد.

---

### 🔹 The `base` Keyword

* `base` مشابه `this` است، اما برای موارد زیر استفاده می‌شود:

  1. دسترسی به پیاده‌سازی **overridden** کلاس پایه
  2. فراخوانی **constructor** کلاس پایه

```csharp
public class House : Asset
{
    public decimal Mortgage;
    public override decimal Liability => base.Liability + Mortgage;
}
```

* با `base` به نسخه کلاس پایه دسترسی غیر virtual داریم و در صورت پنهان شدن عضو هم کار می‌کند.

---

### 🔹 Constructors and Inheritance

* زیرکلاس باید **constructor خودش** را تعریف کند. constructor های کلاس پایه به طور خودکار به کلاس مشتق منتقل نمی‌شوند.
* برای استفاده از constructor کلاس پایه، از `base` استفاده می‌کنیم:

```csharp
public class Baseclass
{
    public int X;
    public Baseclass() { }
    public Baseclass(int x) => X = x;
}

public class Subclass : Baseclass
{
    public Subclass(int x) : base(x) { }
}
```

* **Base-class constructors همیشه اول اجرا می‌شوند** تا ابتدا مقداردهی کلاس پایه کامل شود.

### 🔹 Implicit Calling of the Parameterless Base-Class Constructor

* اگر در **زیرکلاس** از `base` استفاده نکنیم، **constructor بدون پارامتر کلاس پایه** به‌صورت ضمنی فراخوانی می‌شود:

```csharp
public class Baseclass
{
    public int X;
    public Baseclass() { X = 1; }
}

public class Subclass : Baseclass
{
    public Subclass() { Console.WriteLine(X); }  // 1
}
```

* اگر کلاس پایه **constructor بدون پارامتر نداشته باشد**، زیرکلاس مجبور است از `base` استفاده کند:

```csharp
class Baseclass
{
    public Baseclass(int x, int y, int z, string s, DateTime d) { ... }
}

public class Subclass : Baseclass
{
    public Subclass(int x, int y, int z, string s, DateTime d)
        : base(x, y, z, s, d) { ... }
}
```

---

### 🔹 Required Members (C# 11)

* با استفاده از **required members** می‌توان الزام کرد که یک فیلد یا property حتماً هنگام ساخت شیء مقداردهی شود:

```csharp
public class Asset
{
    public required string Name;
}

Asset a1 = new Asset { Name="House" };  // OK
Asset a2 = new Asset();                 // Error
```

* می‌توان با `[SetsRequiredMembers]` این محدودیت را در constructor دور زد:

```csharp
public class Asset
{
    public required string Name;

    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public Asset(string n) => Name = n;
}
```

* این امکان اجازه می‌دهد که هم از **object initializer** و هم از constructor استفاده شود.

---

### 🔹 Constructor and Field Initialization Order

هنگام نمونه‌سازی، **ترتیب مقداردهی** به شکل زیر است:

1. **از زیرکلاس به کلاس پایه**
   a. فیلدهای زیرکلاس مقداردهی می‌شوند
   b. آرگومان‌های فراخوانی constructor پایه ارزیابی می‌شوند

2. **از کلاس پایه به زیرکلاس**
   a. بدنه constructor کلاس پایه اجرا می‌شود
   b. بدنه constructor زیرکلاس اجرا می‌شود

```csharp
public class B
{
    int x = 1;          // 3rd
    public B(int x) { ... } // 4th
}

public class D : B
{
    int y = 1;          // 1st
    public D(int x) : base(x + 1) // 2nd
    {
        ...              // 5th
    }
}
```

---

### 🔹 Inheritance with Primary Constructors

* کلاس‌ها با **primary constructors** می‌توانند به شکل زیر subclass شوند:

```csharp
public class Baseclass(int x) { ... }
public class Subclass(int x, int y) : Baseclass(x) { ... }
```

* این همان کاری است که `base(x)` در constructor معمولی انجام می‌دهد.

---

### 🔹 Overloading and Resolution

* هنگام فراخوانی **overloaded methods**، **نوع دقیق‌تر** ارجحیت دارد:

```csharp
static void Foo(Asset a) { }
static void Foo(House h) { }

House h = new House(...);
Foo(h);  // فراخوانی Foo(House)
```

* اما **تعیین overload** بر اساس نوع **استاتیک** (compile-time) انجام می‌شود:

```csharp
Asset a = new House(...);
Foo(a);  // فراخوانی Foo(Asset)
```

* با استفاده از `dynamic`، تصمیم‌گیری **تا زمان اجرا** به تعویق می‌افتد:

```csharp
Foo((dynamic)a);  // فراخوانی Foo(House) در زمان اجرا
```

---

### 🔹 The `object` Type

* `object` پایه نهایی تمام نوع‌ها است. هر نوعی می‌تواند به `object` **upcast** شود.
* مثال استفاده در یک **Stack ساده**:

```csharp
public class Stack
{
    int position;
    object[] data = new object[10];

    public void Push(object obj) { data[position++] = obj; }
    public object Pop() { return data[--position]; }
}

Stack stack = new Stack();
stack.Push("sausage");
string s = (string)stack.Pop();  // نیاز به downcast
Console.WriteLine(s);            // sausage

stack.Push(3);
int three = (int)stack.Pop();    // boxing/unboxing
```

---

### 🔹 Boxing and Unboxing

* **Boxing** → تبدیل یک value type به object (یا interface):

```csharp
int x = 9;
object obj = x;  // boxing
```

* **Unboxing** → تبدیل object به value type (نیازمند cast صریح):

```csharp
int y = (int)obj;  // unboxing
```

⚠️ نوع اعلام شده باید دقیقاً با نوع واقعی مطابقت داشته باشد، در غیر این صورت `InvalidCastException` رخ می‌دهد:

```csharp
object obj = 9;
long x = (long)obj; // InvalidCastException

long x2 = (int)obj; // صحیح: unboxing سپس تبدیل عددی
```

* **Boxing** و **Unboxing** به نوع سیستم **unified type system** اجازه می‌دهد تا value و reference type ها را با هم کار کند، اما فقط برای **reference conversions** در آرایه‌ها و genericها صادق است:

```csharp
object[] a1 = new string[3]; // OK
object[] a2 = new int[3];    // Error
```

* **Copy semantics**: هنگام boxing/unboxing، مقدار کپی می‌شود، بنابراین تغییر value type اصلی تاثیری بر نسخه boxed ندارد:

```csharp
int i = 3;
object boxed = i;
i = 5;
Console.WriteLine(boxed);  // 3
```

بررسی نوع به‌صورت **استاتیک و زمان اجرا** 🔍🖥️

برنامه‌های C# هم در زمان **کامپایل** (به‌صورت استاتیک) و هم در زمان **اجرای برنامه** (توسط CLR) بررسی نوع می‌شوند.

### بررسی نوع استاتیک

بررسی نوع استاتیک به کامپایلر اجازه می‌دهد که **درستی برنامه شما را بدون اجرای آن** بررسی کند. برای مثال، کد زیر خطا خواهد داد، زیرا کامپایلر نوع‌دهی استاتیک را اعمال می‌کند:

```csharp
int x = "5";  // خطا: نوع صحیح نیست
```

### بررسی نوع زمان اجرا

بررسی نوع در زمان اجرا توسط **CLR** انجام می‌شود، مثلاً وقتی که شما یک **downcast** با استفاده از تبدیل مرجع یا **unboxing** انجام می‌دهید:

```csharp
object y = "5";
int z = (int)y;  // خطای زمان اجرا، downcast ناموفق
```

این بررسی ممکن است، زیرا هر شیء در حافظه heap به‌صورت داخلی **یک توکن نوع کوچک** ذخیره می‌کند. می‌توانید این توکن را با فراخوانی متد `GetType` از شیء دریافت کنید.

---

### متد GetType و عملگر typeof 🏷️

تمام انواع در C# در زمان اجرا با نمونه‌ای از `System.Type` نمایش داده می‌شوند. دو روش اصلی برای دریافت شیء `System.Type` وجود دارد:

* فراخوانی `GetType` روی نمونه
* استفاده از عملگر `typeof` روی نام نوع

`GetType` در زمان اجرا ارزیابی می‌شود، اما `typeof` به‌صورت **استاتیک** در زمان کامپایل ارزیابی می‌شود (وقتی پارامترهای ژنریک در کار باشند، توسط **JIT Compiler** حل می‌شود).

```csharp
Point p = new Point();
Console.WriteLine(p.GetType().Name);             // Point
Console.WriteLine(typeof(Point).Name);          // Point
Console.WriteLine(p.GetType() == typeof(Point)); // True
Console.WriteLine(p.X.GetType().Name);           // Int32
Console.WriteLine(p.Y.GetType().FullName);       // System.Int32

public class Point { public int X, Y; }
```

`System.Type` همچنین متدهایی دارد که دروازه‌ای به **مدل Reflection زمان اجرا** هستند (که در فصل 18 توضیح داده شده است).

---

### متد ToString 📝

متد `ToString` **نمایش متنی پیش‌فرض** یک نمونه از نوع را برمی‌گرداند. این متد در تمام انواع داخلی بازنویسی شده است:

```csharp
int x = 1;
string s = x.ToString(); // s برابر با "1"
```

می‌توانید `ToString` را روی انواع سفارشی خود بازنویسی کنید:

```csharp
Panda p = new Panda { Name = "Petey" };
Console.WriteLine(p);  // Petey

public class Panda
{
    public string Name;
    public override string ToString() => Name;
}
```

اگر `ToString` بازنویسی نشود، نام نوع را برمی‌گرداند.
هنگام فراخوانی یک عضو بازنویسی‌شده مانند `ToString` روی نوع مقداری، **boxing رخ نمی‌دهد**، و تنها وقتی تبدیل انجام دهید، boxing اتفاق می‌افتد:

```csharp
int x = 1;
string s1 = x.ToString();    // فراخوانی روی مقدار غیر-boxed
object box = x;
string s2 = box.ToString();  // فراخوانی روی مقدار boxed
```

---

### اعضای کلاس object 📦

```csharp
public class Object
{
    public Object();
    public extern Type GetType();
    public virtual bool Equals(object obj);
    public static bool Equals(object objA, object objB);
    public static bool ReferenceEquals(object objA, object objB);
    public virtual int GetHashCode();
    public virtual string ToString();
    protected virtual void Finalize();
    protected extern object MemberwiseClone();
}
```

متدهای `Equals`، `ReferenceEquals` و `GetHashCode` در فصل "Equality Comparison" توضیح داده شده‌اند.

---

### Struct ها 📐

یک `struct` شبیه کلاس است با تفاوت‌های کلیدی زیر:

* `struct` یک **value type** است، در حالی که کلاس یک **reference type** است.
* `struct` از **وراثت** پشتیبانی نمی‌کند (به جز به‌صورت ضمنی از `object` یا دقیق‌تر `System.ValueType`).

یک struct می‌تواند تمام اعضایی را داشته باشد که کلاس دارد، به جز **finalizer**. همچنین نمی‌توان اعضای آن را **virtual، abstract یا protected** تعریف کرد.

قبل از C# 10، تعریف **field initializer** و **constructor بدون پارامتر** در struct‌ها ممنوع بود، اما حالا این محدودیت برای **record struct**ها برداشته شده است.

Structها برای زمانی مناسب هستند که **رفتار نوع مقداری (value-type)** مد نظر باشد، مانند انواع عددی که **کپی مقدار** به جای کپی مرجع طبیعی‌تر است. هر نمونه struct نیاز به ایجاد شیء در heap ندارد و این باعث صرفه‌جویی در حافظه می‌شود.

همچنین struct نمی‌تواند `null` باشد و مقدار پیش‌فرض آن، **نمونه‌ای خالی با تمام فیلدها در مقدار پیش‌فرضشان** است.

---

### ساختار ساخت Struct 🏗️

قبل از C# 11، هر فیلد struct باید به‌صورت **صریح در constructor یا field initializer** مقداردهی می‌شد. حالا این محدودیت برداشته شده است.

#### Constructor پیش‌فرض

علاوه بر constructorهای شما، یک **constructor بدون پارامتر ضمنی** همیشه وجود دارد که فیلدها را به‌صورت **bitwise-zero** مقداردهی می‌کند:

```csharp
Point p = new Point();  // p.X و p.Y برابر 0
struct Point { int x, y; }
```

حتی اگر خودتان یک constructor بدون پارامتر تعریف کنید، constructor ضمنی همچنان وجود دارد و می‌توان با استفاده از **default keyword** به آن دسترسی پیدا کرد:

```csharp
Point p1 = new Point();  // p1.x و p1.y برابر 1
Point p2 = default;      // p2.x و p2.y برابر 0

struct Point
{
    int x = 1;
    int y;
    public Point() => y = 1;
}
```

در این مثال، x با **field initializer** و y با **constructor بدون پارامتر** مقداردهی شده است، اما با `default` می‌توان نمونه‌ای ساخت که هر دو مقداردهی را نادیده بگیرد.

---

### استراتژی پیشنهادی برای Struct

بهترین روش این است که struct را طوری طراحی کنید که **مقدار پیش‌فرض آن یک حالت معتبر** باشد و نیازی به مقداردهی اضافی نباشد:

```csharp
struct WebOptions
{
    string protocol;
    public string Protocol
    {
        get => protocol ?? "https";
        set => protocol = value;
    }
}
```

این روش باعث **سادگی و جلوگیری از رفتار گیج‌کننده** هنگام مقداردهی می‌شود. ✅

ساختارهای **خواندنی (Read-Only Structs) و توابع** 📌🖊️

می‌توانید از **modifer `readonly`** روی یک struct استفاده کنید تا اطمینان حاصل شود که **تمام فیلدها فقط خواندنی هستند**. این کار هم نیت شما را واضح می‌کند و هم به کامپایلر اجازه می‌دهد بهینه‌سازی‌های بیشتری انجام دهد:

```csharp
readonly struct Point
{
    public readonly int X, Y;  // X و Y باید readonly باشند
}
```

اگر نیاز دارید `readonly` را با جزئیات بیشتری اعمال کنید، از C# 8 به بعد می‌توانید **توابع struct** را با `readonly` علامت‌گذاری کنید. این کار باعث می‌شود اگر تابع تلاش کند فیلدی را تغییر دهد، **خطای زمان کامپایل** صادر شود:

```csharp
struct Point
{
    public int X, Y;
    public readonly void ResetX() => X = 0;  // خطا!
}
```

اگر یک تابع `readonly`، تابع غیر-readonly دیگری را فراخوانی کند، کامپایلر **هشدار** صادر می‌کند و struct را به‌صورت محافظه‌کارانه کپی می‌کند تا احتمال تغییر ناخواسته جلوگیری شود.

---

### Ref Structs 💎

**Ref struct**ها در C# 7.2 معرفی شدند و ویژگی‌ای خاص برای **Span<T>** و **ReadOnlySpan<T>** فراهم می‌کنند که در فصل 23 توضیح داده شده‌اند (و همچنین Utf8JsonReader در فصل 11). این structها به **میکروبهینه‌سازی حافظه** کمک می‌کنند.

* برخلاف **reference type**ها که همیشه روی heap قرار دارند، **value type**ها در همان مکانی که اعلام شده‌اند زندگی می‌کنند.
* اگر value type به‌صورت پارامتر یا متغیر محلی باشد، روی **stack** قرار می‌گیرد:

```csharp
void SomeMethod()
{
    Point p;  // p روی stack قرار می‌گیرد
}
struct Point { public int X, Y; }
```

* اگر value type به‌عنوان فیلد یک کلاس باشد، روی **heap** قرار می‌گیرد:

```csharp
class MyClass
{
    Point p;  // روی heap، زیرا MyClass روی heap است
}
```

* اضافه کردن `ref` به تعریف struct تضمین می‌کند که struct **تنها روی stack قرار گیرد**. اگر تلاش شود روی heap باشد، خطای زمان کامپایل رخ می‌دهد:

```csharp
var points = new Point[100];  // خطا: کامپایل نمی‌شود
ref struct Point { public int X, Y; }
class MyClass { Point P; }    // خطا: کامپایل نمی‌شود
```

Ref structها به دلیل محدودیت در زندگی روی stack، نمی‌توانند در ویژگی‌هایی شرکت کنند که ممکن است باعث قرارگیری روی heap شوند، مانند **lambda expressionها، iteratorها و توابع async**. همچنین نمی‌توانند در structهای غیر-ref باشند و نمی‌توانند interface پیاده‌سازی کنند (چون ممکن است boxing رخ دهد).

---

### Access Modifiers 🔐

برای **محدود کردن دسترسی** یک نوع یا عضو نوع به سایر نوع‌ها یا اسمبلی‌ها، می‌توان **access modifier** استفاده کرد:

* `public`: دسترسی کامل. برای اعضای enum یا interface پیش‌فرض است.
* `internal`: فقط در اسمبلی جاری یا friend assembly قابل دسترسی است.
* `private`: فقط در داخل نوع جاری قابل دسترسی است.
* `protected`: فقط در داخل نوع جاری یا subclasses قابل دسترسی است.
* `protected internal`: اتحاد `protected` و `internal`.
* `private protected`: اشتراک `protected` و `internal`.
* `file` (C# 11): فقط در همان فایل قابل دسترسی است.

**مثال‌ها:**

```csharp
class Class1 {}            // internal (پیش‌فرض)
public class Class2 {}     // public
```

```csharp
class ClassA { int x; }          // x private
class ClassB { internal int x; } // x internal
```

```csharp
class BaseClass
{
    void Foo() {}           // private
    protected void Bar() {}
}
class Subclass : BaseClass
{
    void Test1() { Foo(); } // خطا: نمی‌توان به Foo دسترسی داشت
    void Test2() { Bar(); } // درست
}
```

---

### Friend Assemblies 🤝

می‌توانید اعضای **internal** را به سایر friend assemblyها با استفاده از attribute زیر در سطح اسمبلی در دسترس قرار دهید:

```csharp
[assembly: InternalsVisibleTo("Friend")]
```

اگر assembly دارای **strong name** باشد، باید کل کلید عمومی 160 بایتی آن را مشخص کنید:

```csharp
[assembly: InternalsVisibleTo("StrongFriend, PublicKey=0024f000048c...")]
```

---

### محدودیت‌ها و Capping دسترسی

* نوع، دسترسی اعضای اعلام شده خود را محدود می‌کند. مثلاً یک نوع internal با اعضای public، در عمل اعضای آن را internal می‌کند.
* هنگام override کردن یک تابع base، **دسترسی باید یکسان باشد**:

```csharp
class BaseClass { protected virtual void Foo() {} }
class Subclass1 : BaseClass { protected override void Foo() {} } // OK
class Subclass2 : BaseClass { public override void Foo() {} }     // خطا
```

کامپایلر از هرگونه **ناسازگاری در access modifiers** جلوگیری می‌کند. به طور مثال، یک subclass می‌تواند کمتر از base class دسترسی داشته باشد، اما نمی‌تواند بیشتر داشته باشد:

```csharp
internal class A {}
public class B : A {}  // خطا
```

**Interfaces (رابطه‌ها) 🧩**

یک **interface** شبیه یک کلاس است، اما تنها رفتار (Behavior) را مشخص می‌کند و **حالت (State)** یا داده نگه نمی‌دارد. بنابراین:

* یک interface تنها می‌تواند **توابع** تعریف کند و نمی‌تواند **فیلد** داشته باشد.
* اعضای interface به‌صورت ضمنی **abstract** هستند. (استثناهایی وجود دارد که در بخش «Default Interface Members» صفحه 151 و «Static Interface Members» صفحه 152 توضیح داده شده‌اند.)
* یک کلاس یا struct می‌تواند چندین interface را پیاده‌سازی کند. در مقابل، یک کلاس تنها می‌تواند از یک کلاس دیگر ارث‌بری کند و struct اصلاً نمی‌تواند ارث‌بری کند (به جز از System.ValueType).

تعریف یک interface شبیه تعریف کلاس است، اما معمولاً هیچ پیاده‌سازی برای اعضای خود ارائه نمی‌دهد، زیرا اعضای آن به‌صورت ضمنی abstract هستند. این اعضا توسط کلاس‌ها و structهایی که interface را پیاده‌سازی می‌کنند، پیاده‌سازی می‌شوند. یک interface می‌تواند تنها شامل **توابع، متدها، properties، events و indexerها** باشد (که دقیقاً همان اعضای کلاس هستند که می‌توانند abstract باشند).

مثال تعریف interface `IEnumerator` در System.Collections:

```csharp
public interface IEnumerator
{
    bool MoveNext();
    object Current { get; }
    void Reset();
}
```

اعضای interface همیشه **ضمنی public** هستند و نمی‌توانند access modifier اعلام کنند. پیاده‌سازی یک interface یعنی **ارائه پیاده‌سازی public** برای تمام اعضای آن:

```csharp
internal class Countdown : IEnumerator
{
    int count = 11;
    public bool MoveNext() => count-- > 0;
    public object Current => count;
    public void Reset() { throw new NotSupportedException(); }
}
```

می‌توانید یک شیء را به هر interface که پیاده‌سازی می‌کند، **به‌صورت ضمنی cast کنید**:

```csharp
IEnumerator e = new Countdown();
while (e.MoveNext())
    Console.Write(e.Current);  // 109876543210
```

حتی اگر Countdown یک کلاس internal باشد، اعضای آن که IEnumerator را پیاده‌سازی می‌کنند می‌توانند **به‌صورت public** فراخوانی شوند.

---

### گسترش یک Interface ➕

Interfaces می‌توانند از سایر interfaceها ارث‌بری کنند:

```csharp
public interface IUndoable { void Undo(); }
public interface IRedoable : IUndoable { void Redo(); }
```

در این مثال، IRedoable تمام اعضای IUndoable را «به ارث می‌برد». یعنی هر نوعی که IRedoable را پیاده‌سازی کند، باید اعضای IUndoable را نیز پیاده‌سازی کند.

---

### پیاده‌سازی صریح Interface 🔒

پیاده‌سازی چند interface گاهی باعث **تداخل در امضاهای اعضا** می‌شود. می‌توانید این تداخل‌ها را با **پیاده‌سازی صریح** حل کنید:

```csharp
interface I1 { void Foo(); }
interface I2 { int Foo(); }

public class Widget : I1, I2
{
    public void Foo()
    {
        Console.WriteLine("Widget's implementation of I1.Foo");
    }
    int I2.Foo()
    {
        Console.WriteLine("Widget's implementation of I2.Foo");
        return 42;
    }
}
```

* چون I1 و I2 دارای Foo با امضاهای متفاوت هستند، Widget **صریحاً I2.Foo** را پیاده‌سازی می‌کند.
* تنها راه فراخوانی یک عضو پیاده‌سازی صریح، **cast به interface** است:

```csharp
Widget w = new Widget();
w.Foo();         // Widget's implementation of I1.Foo
((I1)w).Foo();   // Widget's implementation of I1.Foo
((I2)w).Foo();   // Widget's implementation of I2.Foo
```

یکی دیگر از دلایل پیاده‌سازی صریح، **مخفی کردن اعضای تخصصی و گیج‌کننده** برای استفاده معمولی نوع است، مانند ISerializable.

---

### پیاده‌سازی مجازی Interface Members ⚡

یک عضو interface که به‌طور ضمنی پیاده‌سازی شده است، به‌صورت پیش‌فرض **sealed** است و برای override شدن باید در کلاس پایه **virtual یا abstract** علامت‌گذاری شود:

```csharp
public interface IUndoable { void Undo(); }
public class TextBox : IUndoable
{
    public virtual void Undo() => Console.WriteLine("TextBox.Undo");
}
public class RichTextBox : TextBox
{
    public override void Undo() => Console.WriteLine("RichTextBox.Undo");
}
```

فراخوانی عضو interface از طریق کلاس پایه یا interface، **پیاده‌سازی subclass** را صدا می‌زند:

```csharp
RichTextBox r = new RichTextBox();
r.Undo();                // RichTextBox.Undo
((IUndoable)r).Undo();   // RichTextBox.Undo
((TextBox)r).Undo();     // RichTextBox.Undo
```

یک عضو پیاده‌سازی صریح **نمی‌تواند virtual باشد** و نمی‌توان آن را به‌طور معمول override کرد، اما می‌توان آن را **reimplement** کرد.

---

### پیاده‌سازی مجدد Interface در Subclass 🔄

یک subclass می‌تواند هر عضو interface که قبلاً توسط کلاس پایه پیاده‌سازی شده است را **reimplement** کند. پیاده‌سازی مجدد وقتی که از طریق interface صدا زده شود، جایگزین پیاده‌سازی کلاس پایه می‌شود و فرقی ندارد عضو **virtual** باشد یا خیر.

مثال:

```csharp
public interface IUndoable { void Undo(); }

public class TextBox : IUndoable
{
    void IUndoable.Undo() => Console.WriteLine("TextBox.Undo");
}

public class RichTextBox : TextBox, IUndoable
{
    public void Undo() => Console.WriteLine("RichTextBox.Undo");
}
```

* فراخوانی عضو reimplemented از طریق interface، پیاده‌سازی subclass را صدا می‌زند:

```csharp
RichTextBox r = new RichTextBox();
r.Undo();                 // RichTextBox.Undo      Case 1
((IUndoable)r).Undo();    // RichTextBox.Undo      Case 2
```

اگر TextBox عضو Undo را به‌صورت **ضمنی پیاده‌سازی** می‌کرد، فراخوانی از طریق کلاس پایه نیز ممکن بود و باعث **ناسازگاری** می‌شد:

```csharp
((TextBox)r).Undo();      // TextBox.Undo
```

✅ نکته: پیاده‌سازی مجدد معمولاً **بهترین استراتژی برای override کردن اعضای صریح interface** است، زیرا تنها وقتی که عضو از طریق interface صدا زده شود، اثر می‌کند و از رفتار ناسازگار جلوگیری می‌کند.
**جایگزین‌های پیاده‌سازی مجدد Interface 🔄**

حتی با پیاده‌سازی صریح اعضا، پیاده‌سازی مجدد interface می‌تواند مشکل‌ساز باشد به چند دلیل:

* **زیرکلاس هیچ راهی برای فراخوانی متد کلاس پایه ندارد.**
* **نویسنده کلاس پایه ممکن است انتظار نداشته باشد که یک متد reimplement شود** و پیامدهای احتمالی آن را در نظر نگیرد.

پیاده‌سازی مجدد می‌تواند به عنوان آخرین راه حل در مواقعی که subclassing پیش‌بینی نشده است مفید باشد. اما گزینه بهتر، طراحی کلاس پایه به گونه‌ای است که **هرگز نیاز به reimplementation نباشد**. دو راه برای این کار وجود دارد:

* وقتی یک عضو را به‌طور ضمنی پیاده‌سازی می‌کنید، اگر مناسب است آن را **virtual** علامت بزنید.
* وقتی یک عضو را صریحاً پیاده‌سازی می‌کنید، اگر پیش‌بینی می‌کنید زیرکلاس‌ها ممکن است نیاز به override داشته باشند، از الگوی زیر استفاده کنید:

```csharp
public class TextBox : IUndoable
{
    void IUndoable.Undo()         => Undo();    // فراخوانی متد زیر
    protected virtual void Undo() => Console.WriteLine("TextBox.Undo");
}

public class RichTextBox : TextBox
{
    protected override void Undo() => Console.WriteLine("RichTextBox.Undo");
}
```

اگر انتظار subclassing ندارید، می‌توانید کلاس را **sealed** علامت بزنید تا از پیاده‌سازی مجدد جلوگیری شود.

---

### Interface و Boxing 📦

تبدیل یک **struct** به یک interface باعث **boxing** می‌شود. اما فراخوانی یک عضو ضمنی پیاده‌سازی‌شده روی struct، باعث boxing نمی‌شود:

```csharp
interface I { void Foo(); }
struct S : I { public void Foo() {} }

S s = new S();
s.Foo();         // بدون boxing
I i = s;         // هنگام cast به interface، boxing رخ می‌دهد
i.Foo();
```

---

### Default Interface Members 🛠

از C# 8 به بعد می‌توانید **پیاده‌سازی پیش‌فرض** برای اعضای interface اضافه کنید و اجرای آن را اختیاری کنید:

```csharp
interface ILogger
{
    void Log(string text) => Console.WriteLine(text);
}
```

این ویژگی مفید است اگر بخواهید یک عضو جدید به interface‌ای که در یک کتابخانه پرکاربرد تعریف شده اضافه کنید، بدون اینکه پیاده‌سازی‌های موجود (احتمالاً هزاران مورد) خراب شوند.

* پیاده‌سازی پیش‌فرض همیشه **explicit** است، بنابراین اگر کلاس پیاده‌سازی‌کننده ILogger متد Log را تعریف نکند، تنها راه فراخوانی آن از طریق interface است:

```csharp
class Logger : ILogger { }
((ILogger)new Logger()).Log("message");
```

* این کار از مشکل **multiple implementation inheritance** جلوگیری می‌کند، یعنی اگر یک عضو پیش‌فرض در دو interface که یک کلاس پیاده‌سازی می‌کند اضافه شود، هیچ ابهامی درباره فراخوانی آن وجود ندارد.

---

### Static Interface Members ⚡

یک interface می‌تواند **اعضای static** نیز داشته باشد. دو نوع static وجود دارد:

* **Static nonvirtual interface members**
* **Static virtual/abstract interface members**

بر خلاف اعضای instance، اعضای static به‌طور پیش‌فرض nonvirtual هستند. برای virtual کردن یک عضو static، باید آن را با **static abstract** یا **static virtual** علامت بزنید.

#### Static nonvirtual interface members

این اعضا عمدتاً برای نوشتن **default interface members** مفید هستند. توسط کلاس‌ها یا structها پیاده‌سازی نمی‌شوند، بلکه مستقیماً مصرف می‌شوند. می‌توانند شامل **فیلد** نیز باشند که معمولاً در پیاده‌سازی پیش‌فرض اعضا استفاده می‌شود:

```csharp
interface ILogger
{
    void Log(string text) => Console.WriteLine(Prefix + text);
    static string Prefix = ""; 
}
ILogger.Prefix = "File log: ";
```

* اعضای instance هنوز در interface مجاز نیستند، زیرا هدف interface تعریف **رفتار، نه حالت** است.

#### Static virtual/abstract interface members

اعضای **static virtual/abstract** (از C# 11) امکان **polymorphism استاتیک** را فراهم می‌کنند، که یک ویژگی پیشرفته است.

```csharp
interface ITypeDescribable
{
    static abstract string Description { get; }
    static virtual string Category => null;
}

class CustomerTest : ITypeDescribable
{
    public static string Description => "Customer tests";  // الزامی
    public static string Category    => "Unit testing";    // اختیاری
}
```

* علاوه بر متدها، properties، و events، operatorها و conversions نیز می‌توانند عضو static virtual interface باشند.
* این اعضا از طریق **constrained type parameter** فراخوانی می‌شوند (در بخش Static Polymorphism و Generic Math توضیح داده خواهد شد).

---

### نوشتن Class در مقابل Interface 🏗

راهنما:

* برای نوع‌هایی که **به‌طور طبیعی پیاده‌سازی مشترک دارند**، از کلاس و زیرکلاس استفاده کنید.
* برای نوع‌هایی که **پیاده‌سازی مستقل دارند**، از interface استفاده کنید.

مثال:

```csharp
abstract class Animal {}
abstract class Bird           : Animal {}
abstract class Insect         : Animal {}
abstract class FlyingCreature : Animal {}
abstract class Carnivore      : Animal {}

// کلاس‌های Concrete
class Ostrich : Bird {}
class Eagle   : Bird, FlyingCreature, Carnivore {}  // غیرقانونی
class Bee     : Insect, FlyingCreature {}           // غیرقانونی
class Flea    : Insect, Carnivore {}                // غیرقانونی
```

* کلاس‌های Eagle، Bee، و Flea کامپایل نمی‌شوند، زیرا **ارث‌بری چندگانه از کلاس‌ها مجاز نیست**.
* راه حل: برخی از نوع‌ها را به interface تبدیل کنیم.

قاعده عمومی:

* Insect و Bird پیاده‌سازی مشترک دارند → کلاس باقی بمانند
* FlyingCreature و Carnivore دارای مکانیزم‌های مستقل هستند → تبدیل به interface:

```csharp
interface IFlyingCreature {}
interface ICarnivore      {}
```

* مثال واقعی: Bird و Insect می‌توانند معادل Windows control و web control باشند. FlyingCreature و Carnivore می‌توانند معادل IPrintable و IUndoable باشند.
**Enums 🔢**

یک **enum** نوع ویژه‌ای از value type است که به شما امکان می‌دهد گروهی از **ثابت‌های عددی نام‌گذاری‌شده** را تعریف کنید. به عنوان مثال:

```csharp
public enum BorderSide { Left, Right, Top, Bottom }
```

می‌توانیم از این enum به این شکل استفاده کنیم:

```csharp
BorderSide topSide = BorderSide.Top;
bool isTop = (topSide == BorderSide.Top);   // true
```

هر عضو enum دارای یک **مقدار عددی زمینه‌ای** است. به‌طور پیش‌فرض:

* مقادیر زمینه‌ای از نوع `int` هستند.
* ثابت‌ها به ترتیب اعلام‌شده، به صورت خودکار 0، 1، 2… اختصاص می‌یابند.

می‌توانید نوع عددی جایگزین نیز مشخص کنید:

```csharp
public enum BorderSide : byte { Left, Right, Top, Bottom }
```

همچنین می‌توانید برای هر عضو مقدار زمینه‌ای صریح تعیین کنید:

```csharp
public enum BorderSide : byte { Left=1, Right=2, Top=10, Bottom=11 }
```

کامپایلر به شما اجازه می‌دهد تنها برخی از اعضا را مقداردهی کنید؛ اعضای بدون مقدار، از آخرین مقدار صریح افزایشی ادامه می‌یابند.

---

### تبدیل‌های Enum 🔄

می‌توانید یک نمونه enum را به نوع عددی زمینه‌ای و برعکس تبدیل کنید با **explicit cast**:

```csharp
int i = (int) BorderSide.Left;
BorderSide side = (BorderSide) i;
bool leftOrRight = (int) side <= 2;
```

همچنین می‌توانید یک enum را به enum دیگری تبدیل کنید:

```csharp
public enum HorizontalAlignment
{
    Left = BorderSide.Left,
    Right = BorderSide.Right,
    Center
}

HorizontalAlignment h = (HorizontalAlignment) BorderSide.Right;
// مشابه:
HorizontalAlignment h = (HorizontalAlignment)(int) BorderSide.Right;
```

* عدد 0 در یک عبارت enum به‌طور ویژه‌ای رفتار می‌کند و نیازی به cast ندارد:

```csharp
BorderSide b = 0;    // بدون cast
if (b == 0) ...
```

دلایل ویژه بودن 0:

* عضو اول enum اغلب به عنوان مقدار **default** استفاده می‌شود.
* برای enumهای ترکیبی، 0 به معنای **no flags** است.

---

### Flags Enums 🏴

می‌توانید اعضای enum را با هم ترکیب کنید. برای جلوگیری از ابهام، اعضای **combinable enum** باید مقادیر صریح داشته باشند، معمولاً به صورت توان‌های دو:

```csharp
[Flags]
enum BorderSides { None=0, Left=1, Right=2, Top=4, Bottom=8 }
```

یا:

```csharp
enum BorderSides { None=0, Left=1, Right=1<<1, Top=1<<2, Bottom=1<<3 }
```

برای کار با مقادیر ترکیبی از **عملگرهای بیتی** مانند `|` و `&` استفاده می‌کنیم:

```csharp
BorderSides leftRight = BorderSides.Left | BorderSides.Right;
if ((leftRight & BorderSides.Left) != 0)
    Console.WriteLine("Includes Left");  // Includes Left
```

```csharp
string formatted = leftRight.ToString();   // "Left, Right"
BorderSides s = BorderSides.Left;
s |= BorderSides.Right;
Console.WriteLine(s == leftRight);   // True
s ^= BorderSides.Right;               // تعویض BorderSides.Right
Console.WriteLine(s);                 // Left
```

* طبق convention، اگر اعضای enum قابل ترکیب هستند، همیشه **Flags attribute** را اضافه کنید.
* نام enum ترکیبی معمولاً به **جمع** نام‌گذاری می‌شود.
* می‌توانید **اعضای ترکیبی** را در خود declaration تعریف کنید:

```csharp
[Flags]
enum BorderSides
{
    None=0,
    Left=1, Right=1<<1, Top=1<<2, Bottom=1<<3,
    LeftRight = Left | Right, 
    TopBottom = Top | Bottom,
    All       = LeftRight | TopBottom
}
```

---

### عملگرهای Enum ⚙️

عملگرهایی که با enums کار می‌کنند:

```
=   ==   !=   <   >   <=   >=   +   -   ^  &  |   ~
+=  -=  ++  --   sizeof
```

* عملگرهای بیتی، محاسباتی و مقایسه‌ای نتیجه را روی مقادیر عددی زمینه‌ای انجام می‌دهند.
* جمع بین یک enum و نوع عددی مجاز است، اما بین دو enum مجاز نیست.

---

### مسائل Type-Safety ⚠️

فرض کنید داریم:

```csharp
public enum BorderSide { Left, Right, Top, Bottom }
```

چون enum می‌تواند به نوع عددی و بالعکس تبدیل شود، ممکن است مقدار آن خارج از محدوده اعضای قانونی باشد:

```csharp
BorderSide b = (BorderSide)12345;
Console.WriteLine(b);  // 12345
```

عملگرهای بیتی و محاسباتی نیز می‌توانند مقادیر نامعتبر تولید کنند:

```csharp
BorderSide b = BorderSide.Bottom;
b++;  // بدون خطا
```

* مقدار نامعتبر می‌تواند کد زیر را خراب کند:

```csharp
void Draw(BorderSide side)
{
    if      (side == BorderSide.Left)  {...}
    else if (side == BorderSide.Right) {...}
    else if (side == BorderSide.Top)   {...}
    else                               {...} // فرض BorderSide.Bottom
}
```

راه حل‌ها:

1. اضافه کردن یک else دیگر:

```csharp
...
else if (side == BorderSide.Bottom) ...
else throw new ArgumentException("Invalid BorderSide: " + side, "side");
```

2. بررسی صریح مقدار enum با `Enum.IsDefined`:

```csharp
BorderSide side = (BorderSide)12345;
Console.WriteLine(Enum.IsDefined(typeof(BorderSide), side));   // False
```

* متأسفانه، `Enum.IsDefined` برای **flagged enums** کار نمی‌کند. اما می‌توان از روش کمکی زیر استفاده کرد:

```csharp
bool IsFlagDefined(Enum e)
{
    decimal d;
    return !decimal.TryParse(e.ToString(), out d);
}
```

---

### Nested Types 🏗

یک **nested type** در داخل محدوده یک نوع دیگر تعریف می‌شود:

```csharp
public class TopLevel
{
    public class Nested { }               // کلاس تو در تو
    public enum Color { Red, Blue, Tan }  // enum تو در تو
}
```

ویژگی‌های nested type:

* می‌تواند به **private members** نوع enclosing دسترسی داشته باشد.
* می‌توانید از تمام access modifierها استفاده کنید.
* دسترسی پیش‌فرض برای nested type **private** است نه internal.
* دسترسی به یک nested type از خارج نیازمند qualification با نام enclosing type است:

```csharp
TopLevel.Color color = TopLevel.Color.Red;
```

* همه نوع‌ها (class، struct، interface، delegate و enum) می‌توانند nested باشند.

مثال دسترسی به member خصوصی از nested type:

```csharp
public class TopLevel
{
    static int x;
    class Nested
    {
        static void Foo() { Console.WriteLine(TopLevel.x); }
    }
}
```

مثال استفاده از `protected` روی nested type:

```csharp
public class TopLevel
{
    protected class Nested { }
}
public class SubTopLevel : TopLevel
{
    static void Foo() { new TopLevel.Nested(); }
}
```

Nested types توسط کامپایلر نیز برای ایجاد **کلاس‌های خصوصی** که state را برای iteratorها و anonymous methods ذخیره می‌کنند، استفاده می‌شوند.

---

### Generics ⚙️

اگر هدف از استفاده از nested type تنها جلوگیری از شلوغی namespace است، بهتر است از **nested namespace** استفاده کنید.

C# دو مکانیزم برای نوشتن کد قابل استفاده مجدد در نوع‌های مختلف دارد:

* **Inheritance**: بازگویی قابلیت reuse با base type

* **Generics**: بازگویی قابلیت reuse با template که شامل placeholder typeهاست

* Generics می‌توانند **type safety** را افزایش دهند و نیاز به casting و boxing را کاهش دهند.

#### Generic Types 🧩

C# generics و C++ templates مشابه هستند اما رفتار متفاوتی دارند.

یک **generic type** پارامترهای نوعی (placeholder) تعریف می‌کند که مصرف‌کننده generic نوع واقعی را جایگزین می‌کند. مثال **Stack<T>**:

```csharp
public class Stack<T>
{
    int position;
    T[] data = new T[100];

    public void Push(T obj) => data[position++] = obj;
    public T Pop() => data[--position];
}
```

استفاده از Stack<T>:

```csharp
var stack = new Stack<int>();
stack.Push(5);
stack.Push(10);
int x = stack.Pop();  // 10
int y = stack.Pop();  // 5
```

* `Stack<int>` پارامتر نوعی `T` را با `int` جایگزین می‌کند و نوع بسته‌ای ایجاد می‌کند.
* تلاش برای push کردن یک `string` روی `Stack<int>` باعث خطای compile-time می‌شود.

```csharp
// تعریف مشابه Stack<int>
public class ###
{
    int position;
    int[] data = new int[100];

    public void Push(int obj) => data[position++] = obj;
    public int Pop() => data[--position];
}
```

* `Stack<T>` یک **open type** است، و `Stack<int>` یک **closed type**.
* در زمان اجرا، تمام نمونه‌های generic type بسته هستند و placeholderها پر می‌شوند.

مثال نامعتبر:

```csharp
var stack = new Stack<T>();   // غیرقانونی: T چیست؟
```

* اما اگر در یک کلاس یا متدی که خودش T را تعریف کرده باشد:

```csharp
public class Stack<T>
{
    ...
    public Stack<T> Clone()
    {
        Stack<T> clone = new Stack<T>();   // قانونی
        ...
    }
}
```
### Why Generics Exist 🧩

Generics در C# وجود دارند تا بتوانید کدی **قابل استفاده مجدد برای انواع مختلف** بنویسید.

فرض کنید می‌خواهیم یک stack برای اعداد صحیح داشته باشیم و generic نداریم. دو راه وجود دارد:

1. ایجاد نسخه‌های جداگانه کلاس برای هر نوع مورد نیاز: `IntStack`, `StringStack` و غیره → باعث **تکرار زیاد کد** می‌شود.
2. استفاده از `object` به عنوان نوع عناصر:

```csharp
public class ObjectStack
{
    int position;
    object[] data = new object[10];
    public void Push(object obj) => data[position++] = obj;
    public object Pop() => data[--position];
}
```

اما **ObjectStack** معایبی دارد:

* نیاز به **boxing** و **downcasting** برای نوع‌های value type.
* نوع نادرست می‌تواند وارد شود بدون اینکه کامپایلر خطا بدهد:

```csharp
ObjectStack stack = new ObjectStack();
stack.Push("s");           // Wrong type, no compile error
int i = (int)stack.Pop();  // Runtime error
```

راه حل: استفاده از **generics**.

```csharp
Stack<int> stack = new Stack<int>();
```

* مانند ObjectStack: یکبار نوشته شده و می‌تواند با هر نوع کار کند.
* مانند IntStack: برای نوع خاص `T` تخصصی شده است.
* مزیت: **ایمنی نوعی** و کاهش نیاز به cast و boxing.

> ObjectStack عملاً معادل `Stack<object>` است.

---

### Generic Methods 🔄

متدهای generic پارامترهای نوعی خود را در **signature متد** معرفی می‌کنند.
مثال: swap دو مقدار از هر نوع T

```csharp
static void Swap<T>(ref T a, ref T b)
{
    T temp = a;
    a = b;
    b = temp;
}
```

استفاده:

```csharp
int x = 5, y = 10;
Swap(ref x, ref y);
```

* معمولاً نیازی به مشخص کردن نوع نیست، کامپایلر آن را استنتاج می‌کند.
* اگر ابهام باشد:

```csharp
Swap<int>(ref x, ref y);
```

> یک متد در داخل generic type، مگر اینکه type parameter جدید معرفی کند، **متد generic** محسوب نمی‌شود.

---

### Declaring Type Parameters 📦

پارامترهای نوع می‌توانند در **کلاس‌ها، structها، interfaceها، delegateها و متدها** تعریف شوند.
مثال:

```csharp
public struct Nullable<T>
{
    public T Value { get; }
}

class Dictionary<TKey, TValue> {...}
var myDict = new Dictionary<int,string>();
```

* می‌توان چند پارامتر نوعی داشت: `Dictionary<TKey, TValue>`
* نامگذاری: پارامتر تک نوعی → `T`، چند پارامتر → `TKey, TValue` و غیره.

#### typeof و Unbound Generic Types

* **Open generic types** در زمان اجرا وجود ندارند مگر به شکل **Type object**:

```csharp
class A<T> {}
Type a1 = typeof(A<>);   // Unbound
Type a2 = typeof(A<,>);  // برای چند پارامتر
Type a3 = typeof(A<int,int>); // Closed
```

---

### Default Generic Value ⚙️

کلمه کلیدی `default` برای گرفتن مقدار پیش‌فرض پارامتر نوعی استفاده می‌شود:

```csharp
static void Zap<T>(T[] array)
{
    for (int i = 0; i < array.Length; i++)
        array[i] = default(T);
}

// C# 7.1 به بعد:
array[i] = default;
```

* مقدار پیش‌فرض **reference type** → `null`
* مقدار پیش‌فرض **value type** → صفر بیت به بیت (bitwise-zero)

---

### Generic Constraints 🛡️

به‌طور پیش‌فرض می‌توان هر نوعی را جایگزین T کرد، اما **constraints** محدودیت‌هایی اضافه می‌کنند:

```csharp
where T : base-class       // Must derive from a base class
where T : interface        // Must implement an interface
where T : class            // Reference type
where T : struct           // Value type (excludes Nullable)
where T : unmanaged        // Simple value type without references
where T : new()            // Parameterless constructor
where U : T                // U must derive from T
where T : notnull          // Non-nullable (C# 8+)
```

مثال:

```csharp
class SomeClass {}
interface Interface1 {}

class GenericClass<T, U>
    where T : SomeClass, Interface1
    where U : new()
{
    ...
}
```

* هدف اصلی constraints: **امکان انجام عملیاتی که بدون آن غیرممکن است**.
* مثال: `T:Foo` اجازه می‌دهد T را مثل Foo رفتار دهید، `T:new()` اجازه ساخت instance از T می‌دهد.

---

### Example: Max Method Using Constraints ✅

استفاده از interface constraint `IComparable<T>`:

```csharp
static T Max<T>(T a, T b) where T : IComparable<T>
{
    return a.CompareTo(b) > 0 ? a : b;
}

int z = Max(5, 10);               // 10
string last = Max("ant", "zoo");  // "zoo"
```

* C# 11: interface constraint اجازه فراخوانی **static virtual/abstract members** می‌دهد.

---

### Other Constraints Examples

* **class/struct constraint**: مشخص می‌کند T یک reference type یا value type است.
* **unmanaged constraint**: T باید یک value type ساده یا struct بدون reference types باشد.
* **new() constraint**: اجازه ساخت instance با `new T()`:

```csharp
static void Initialize<T>(T[] array) where T : new()
{
    for (int i = 0; i < array.Length; i++)
        array[i] = new T();
}
```

* **naked type constraint**: یک type parameter باید از دیگری مشتق شود:

```csharp
class Stack<T>
{
    Stack<U> FilteredStack<U>() where U : T {...}
}
```
### Subclassing Generic Types 🧬

یک کلاس generic می‌تواند مانند کلاس معمولی subclass شود. چند حالت وجود دارد:

1. **SubClass با همان پارامتر نوع باز باقی می‌ماند**:

```csharp
class Stack<T> { ... }
class SpecialStack<T> : Stack<T> { ... }
```

2. **SubClass نوع پارامتر را به یک نوع مشخص می‌بندد**:

```csharp
class IntStack : Stack<int> { ... }
```

3. **SubClass می‌تواند پارامتر نوع جدید معرفی کند**:

```csharp
class List<T> { ... }
class KeyedList<T, TKey> : List<T> { ... }
```

* در واقع، تمام type argumentها در subclass **fresh** هستند و می‌توان نام‌های جدید و معنادارتری برای آنها انتخاب کرد:

```csharp
class KeyedList<TElement, TKey> : List<TElement> { ... }
```

---

### Self-Referencing Generic Declarations 🔄

یک نوع می‌تواند خودش را به عنوان **concrete type** معرفی کند:

```csharp
public interface IEquatable<T> { bool Equals(T obj); }

public class Balloon : IEquatable<Balloon>
{
    public string Color { get; set; }
    public int CC { get; set; }
    public bool Equals(Balloon b)
    {
        if (b == null) return false;
        return b.Color == Color && b.CC == CC;
    }
}
```

همچنین این‌ها قانونی هستند:

```csharp
class Foo<T> where T : IComparable<T> { ... }
class Bar<T> where T : Bar<T> { ... }
```

---

### Static Data in Generic Types 💾

* **Static data** منحصر به هر **closed type** است:

```csharp
class Bob<T> { public static int Count; }

Console.WriteLine(++Bob<int>.Count);    // 1
Console.WriteLine(++Bob<int>.Count);    // 2
Console.WriteLine(++Bob<string>.Count); // 1
Console.WriteLine(++Bob<object>.Count); // 1
```

---

### Type Parameters and Conversions ⚖️

C# چند نوع تبدیل را پشتیبانی می‌کند:

* Numeric
* Reference
* Boxing/unboxing
* Custom (operator overloading)

اما با **generic type parameters**، نوع دقیق در زمان کامپایل مشخص نیست، پس ممکن است **ابهام** ایجاد شود.

مثال:

```csharp
StringBuilder Foo<T>(T arg)
{
    if (arg is StringBuilder)
        return (StringBuilder)arg; // خطا در کامپایل
}
```

راه حل‌ها:

1. استفاده از `as` (بی‌ابهام):

```csharp
StringBuilder sb = arg as StringBuilder;
if (sb != null) return sb;
```

2. یا ابتدا cast به `object` و سپس به نوع مورد نظر:

```csharp
return (StringBuilder)(object)arg;
```

> Unboxing هم می‌تواند ابهام ایجاد کند:
>
> ```csharp
> int Foo<T>(T x) => (int)x;  // خطای کامپایل
> int Foo<T>(T x) => (int)(object)x; // صحیح
> ```

---

### Covariance & Contravariance 🔁

* **Covariance**: اگر `A` به `B` قابل تبدیل باشد، نوع generic `X<A>` می‌تواند به `X<B>` تبدیل شود.
* فقط برای **implicit reference conversions** اعمال می‌شود (مثل subclass یا interface implementation).
* **Classes** پشتیبانی نمی‌کنند، اما **interfaces و delegates** و **arrays** پشتیبانی می‌کنند.

مثال عدم covariance:

```csharp
class Animal {}
class Bear : Animal {}
class Camel : Animal {}

Stack<Bear> bears = new Stack<Bear>();
Stack<Animal> animals = bears;  // خطای کامپایل
animals.Push(new Camel());      // اگر مجاز بود، runtime error
```

راه حل‌ها:

1. متد generic با constraint بنویسیم:

```csharp
class ZooCleaner
{
    public static void Wash<T>(Stack<T> animals) where T : Animal { ... }
}

Stack<Bear> bears = new Stack<Bear>();
ZooCleaner.Wash(bears);
```

2. یا Stack<T> را روی interface با **covariant type parameter** تعریف کنیم.

---

### Arrays و Covariance ⚠️

* Arrayها از نظر تاریخی covariant هستند:

```csharp
Bear[] bears = new Bear[3];
Animal[] animals = bears;  // OK
```

* اما **assignments نادرست ممکن است runtime error بدهد**:

```csharp
animals[0] = new Camel();  // Runtime error
```
### اعلام یک پارامتر نوع کوواریانت 📐

پارامترهای نوع در **interfaces** و **delegates** می‌توانند با علامت‌گذاری با **modifer `out`** کوواریانت اعلام شوند. این **modifer** تضمین می‌کند که برخلاف آرایه‌ها، پارامترهای نوع کوواریانت کاملاً **ایمن از نظر نوع (type-safe)** هستند.

می‌توانیم این موضوع را با کلاس `Stack<T>` خود نشان دهیم، به طوری که آن را به شکل زیر پیاده‌سازی کنیم:

```csharp
public interface IPoppable<out T> { T Pop(); }
```

علامت `out` روی `T` نشان می‌دهد که `T` تنها در **موقعیت‌های خروجی** (مثلاً نوع بازگشتی متدها) استفاده می‌شود. این علامت، پارامتر نوع را کوواریانت کرده و اجازه می‌دهد کارهای زیر را انجام دهیم:

```csharp
var bears = new Stack<Bear>();
bears.Push(new Bear());
// Bears پیاده‌سازی IPoppable<Bear> را دارد. می‌توانیم آن را به IPoppable<Animal> تبدیل کنیم:
IPoppable<Animal> animals = bears;   // قانونی
Animal a = animals.Pop();
```

تبدیل از `bears` به `animals` توسط کامپایلر مجاز است، زیرا **پارامتر نوع کوواریانت است**. این تبدیل **ایمن از نظر نوع** است، زیرا موقعیتی که کامپایلر می‌خواهد از آن جلوگیری کند—قرار دادن یک **Camel** در استک—امکان‌پذیر نیست، چرا که هیچ راهی برای فرستادن Camel به یک interface که `T` تنها در خروجی ظاهر می‌شود، وجود ندارد.

💡 کوواریانس (و کانترکوواریانس) در interfaces معمولاً **مصرف می‌شود** و کمتر پیش می‌آید که نیاز باشد **interfaces کوواریانت بنویسید**.

نکته جالب: پارامترهای متد که با `out` علامت‌گذاری شده‌اند، به دلیل محدودیت‌های CLR، **قابل کوواریانس نیستند**.

می‌توانیم از قابلیت **cast کوواریانت** برای حل مشکل **قابلیت استفاده مجدد (reusability)** که قبلاً توضیح داده شد، استفاده کنیم:

```csharp
public class ZooCleaner
{
    public static void Wash(IPoppable<Animal> animals) { ... }
}
```

💡 **interfaces `IEnumerator<T>` و `IEnumerable<T>`** که در فصل ۷ توضیح داده شده‌اند، دارای پارامتر نوع کوواریانت `T` هستند. این باعث می‌شود بتوانید، برای مثال، `IEnumerable<string>` را به `IEnumerable<object>` تبدیل کنید.

کامپایلر **خطا ایجاد می‌کند** اگر پارامتر نوع کوواریانت را در یک **موقعیت ورودی** (مثلاً پارامتر متد یا property قابل نوشتن) استفاده کنید.

---

### کوواریانس (و کانترکوواریانس) و محدودیت‌ها ⚠️

کوواریانس و کانترکوواریانس **فقط برای عناصری با تبدیل مرجع (reference conversion)** کار می‌کند—نه برای تبدیل‌های boxing. این قانون هم برای **variance پارامتر نوع** و هم برای **variance آرایه‌ها** اعمال می‌شود. بنابراین اگر متدی پارامتری از نوع `IPoppable<object>` بپذیرد، می‌توان آن را با `IPoppable<string>` فراخوانی کرد اما **نه با `IPoppable<int>`**.

---

### کانترکوواریانس 🔄

فرض کنید `A` قابلیت تبدیل مرجع به `B` را دارد. اگر `X<A>` قابلیت تبدیل به `X<B>` را داشته باشد، پارامتر نوع `X` کوواریانت است.
کانترکوواریانس زمانی است که بتوانید **در جهت معکوس** تبدیل کنید—از `X<B>` به `X<A>`. این زمانی امکان‌پذیر است که **پارامتر نوع تنها در موقعیت‌های ورودی استفاده شود** و با `in` علامت‌گذاری شود.

با گسترش مثال قبلی، فرض کنید کلاس `Stack<T>` این interface را پیاده‌سازی می‌کند:

```csharp
public interface IPushable<in T> { void Push(T obj); }
```

اکنون می‌توانیم قانونی عمل کنیم:

```csharp
IPushable<Animal> animals = new Stack<Animal>();
IPushable<Bear> bears = animals;    // قانونی
bears.Push(new Bear());
```

هیچ عضوی در `IPushable` نوع `T` را خروجی نمی‌دهد، بنابراین نمی‌توانیم با تبدیل `animals` به `bears` دچار مشکل شویم (مثلاً راهی برای Pop کردن وجود ندارد).

کلاس `Stack<T>` ما می‌تواند همزمان **`IPushable<T>` و `IPoppable<T>`** را پیاده‌سازی کند، حتی اگر `T` در دو interface با **annotaion‌های variance مخالف** باشد! این کار امکان‌پذیر است زیرا **variance باید از طریق interface اعمال شود، نه کلاس**؛ بنابراین، قبل از انجام تبدیل variant، باید از دیدگاه IPoppable یا IPushable متعهد شوید. این دیدگاه سپس شما را به عملیات قانونی تحت قوانین variance محدود می‌کند.

💡 این توضیح می‌دهد چرا **کلاس‌ها اجازه پارامتر نوع variant ندارند**: پیاده‌سازی‌های واقعی معمولاً نیاز دارند داده‌ها **در هر دو جهت جریان داشته باشند**.

---

### مثال دیگر: IComparer<in T> 🧮

در فضای نام **System**:

```csharp
public interface IComparer<in T>
{
    // مقدار نسبت ترتیب a و b را برمی‌گرداند
    int Compare(T a, T b);
}
```

چون این interface دارای پارامتر نوع کانترکوواریانت `T` است، می‌توانیم از یک `IComparer<object>` برای مقایسه دو رشته استفاده کنیم:

```csharp
var objectComparer = Comparer<object>.Default; // objectComparer پیاده‌سازی IComparer<object>
IComparer<string> stringComparer = objectComparer;
int result = stringComparer.Compare("Brett", "Jemaine");
```

همانند کوواریانس، کامپایلر **خطا گزارش می‌دهد** اگر پارامتر نوع کانترکوواریانت را در **موقعیت خروجی** (مثلاً مقدار بازگشتی یا property قابل خواندن) استفاده کنید.

---

### C# Generics در مقایسه با C++ Templates ⚙️

**Generics در C#** مشابه **templates در C++** هستند اما به شکل متفاوتی عمل می‌کنند. در هر دو حالت، یک **ترکیب بین تولیدکننده و مصرف‌کننده** صورت می‌گیرد که در آن **placeholder typeهای تولیدکننده** توسط مصرف‌کننده پر می‌شوند.

با این حال، در **C# generics**، تولیدکننده‌ها (open types مانند `List<T>`) می‌توانند به یک **کتابخانه کامپایل شوند** (مثل `mscorlib.dll`) چون ترکیب تولیدکننده و مصرف‌کننده که منجر به closed typeها می‌شود، **فقط در زمان اجرا اتفاق می‌افتد**.

در C++ templates، این ترکیب در **زمان کامپایل** انجام می‌شود؛ بنابراین **کتابخانه‌های template به صورت DLL منتشر نمی‌شوند** و فقط به عنوان **کد منبع** وجود دارند. این باعث می‌شود بررسی و ایجاد dynamic نوع‌های پارامتری دشوار باشد.

---

### مثال Max در C# و C++

در C#:

```csharp
static T Max<T>(T a, T b) where T : IComparable<T>
  => a.CompareTo(b) > 0 ? a : b;
```

چرا نمی‌توانیم اینطور پیاده کنیم؟

```csharp
static T Max<T>(T a, T b)
  => (a > b ? a : b); // خطای کامپایل
```

چون Max باید **یک بار کامپایل شود و برای همه نوع‌های T کار کند**. کامپایل نمی‌تواند موفق شود زیرا عملگر `>` معنای یکنواختی برای همه نوع‌ها ندارد—در واقع، همه Tها حتی عملگر `>` ندارند.

در مقابل، در C++:

```cpp
template <class T> 
T Max(T a, T b)
{
    return a > b ? a : b;
}
```

این کد **برای هر مقدار T جداگانه کامپایل می‌شود** و معنای `>` را برای نوع خاص خود می‌گیرد، و اگر نوعی از `>` پشتیبانی نکند، کامپایل خطا می‌دهد.


</div>