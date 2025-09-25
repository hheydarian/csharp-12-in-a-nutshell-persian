# فصل بیست  و سوم:  یکپارچه‌سازی با Native و COM 

این فصل توضیح می‌دهد چگونه با کتابخانه‌های Native (غیرمدیریت‌شده) Dynamic-Link (DLL) و کامپوننت‌های Component Object Model (COM) یکپارچه شوید. مگر اینکه خلاف آن ذکر شده باشد، انواع داده‌ای که در این فصل آمده‌اند در فضای نام **System** یا **System.Runtime.InteropServices** وجود دارند.

---

#### فراخوانی DLLهای Native 📦

**P/Invoke**، کوتاه شده‌ی **Platform Invocation Services**، به شما اجازه می‌دهد به توابع، ساختارها و callback‌ها در DLLهای غیرمدیریت‌شده (کتابخانه‌های مشترک در Unix) دسترسی پیدا کنید.

برای مثال، تابع `MessageBox` که در DLL ویندوز **user32.dll** تعریف شده است به شکل زیر است:

```c
int MessageBox(HWND hWnd, LPCTSTR lpText, LPCTSTR lpCaption, UINT uType);
```

می‌توانید این تابع را مستقیماً با تعریف یک متد **static** با همان نام، استفاده از کلمه کلیدی `extern` و افزودن attribute `DllImport` فراخوانی کنید:

```csharp
using System;
using System.Runtime.InteropServices;

MessageBox(IntPtr.Zero, 
           "Please do not press this again.", "Attention", 0);

[DllImport("user32.dll")]
static extern int MessageBox(IntPtr hWnd, string text, string caption, int type);
```

کلاس‌های `MessageBox` در فضای نام‌های **System.Windows** و **System.Windows.Forms** خودشان متدهای مشابه غیرمدیریت‌شده را فراخوانی می‌کنند.

نمونه‌ای از `DllImport` برای **Ubuntu Linux**:

```csharp
Console.WriteLine($"User ID: {getuid()}");

[DllImport("libc")]
static extern uint getuid();
```

CLR شامل یک **marshaler** است که می‌داند چگونه پارامترها و مقادیر بازگشتی بین انواع .NET و انواع غیرمدیریت‌شده تبدیل شوند. در مثال ویندوز، پارامترهای `int` مستقیماً به عدد صحیح چهار بایتی که تابع انتظار دارد تبدیل می‌شوند و پارامترهای `string` به آرایه‌های Unicode پایان‌یافته با null (UTF-16) تبدیل می‌شوند.
`IntPtr` یک struct است که برای پوشش یک **handle** غیرمدیریت‌شده طراحی شده؛ در پلتفرم‌های ۳۲ بیتی، ۳۲ بیت و در پلتفرم‌های ۶۴ بیتی، ۶۴ بیت عرض دارد. تبدیل مشابهی در Unix نیز انجام می‌شود. (از C# 9 به بعد، می‌توانید از نوع `nint` هم استفاده کنید که به `IntPtr` نگاشت می‌شود.)

---

#### Marshaling انواع و پارامترها ⚙️

##### Marshaling انواع رایج

در سمت غیرمدیریت‌شده، ممکن است برای نمایش یک نوع داده بیش از یک روش وجود داشته باشد.
برای مثال، یک رشته (`string`) می‌تواند شامل کاراکترهای تک‌بایتی ANSI یا کاراکترهای Unicode UTF-16 باشد و طول آن می‌تواند با پیش‌وند مشخص شود، یا null-terminated باشد، یا طول ثابت داشته باشد.

با استفاده از attribute `MarshalAs` می‌توانید به marshaler CLR مشخص کنید کدام حالت استفاده شود تا تبدیل صحیح انجام گیرد. مثال:

```csharp
[DllImport("...")]
static extern int Foo([MarshalAs(UnmanagedType.LPStr)] string s);
```

enum `UnmanagedType` شامل تمام انواع Win32 و COM است که marshaler آن‌ها را می‌شناسد. در این مثال، marshaler به ترجمه به `LPStr` دستور داده شد، که یک رشته تک‌بایتی ANSI پایان‌یافته با null است.

در سمت .NET نیز شما می‌توانید نوع داده‌ای که استفاده می‌کنید را انتخاب کنید. **Handles** غیرمدیریت‌شده، برای مثال، می‌توانند به `IntPtr`، `int`، `uint`، `long` یا `ulong` نگاشت شوند.

بیشتر handles غیرمدیریت‌شده یک آدرس یا pointer را در بر دارند و بنابراین برای سازگاری با سیستم‌عامل‌های ۳۲ و ۶۴ بیتی باید به `IntPtr` نگاشت شوند. یک مثال معمول، `HWND` است.

اغلب در توابع Win32 و POSIX، با پارامترهای عدد صحیح مواجه می‌شوید که مجموعه‌ای از **constants** را می‌پذیرند، که در فایل هدر C++ مانند `WinUser.h` تعریف شده‌اند. به جای تعریف آن‌ها به عنوان constants ساده در C#، می‌توانید آن‌ها را در یک **enum** تعریف کنید. استفاده از enum باعث تمیزتر شدن کد و افزایش ایمنی نوعی می‌شود. نمونه‌ای در بخش «Shared Memory» در صفحه ۹۹۵ ارائه شده است.

---

هنگام نصب **Microsoft Visual Studio**، حتماً فایل‌های هدر C++ را نصب کنید—حتی اگر هیچ مورد دیگری از دسته C++ انتخاب نکرده باشید. اینجا جایی است که تمام constants بومی Win32 تعریف شده‌اند. سپس می‌توانید همه فایل‌های هدر را با جستجوی `*.h` در دایرکتوری برنامه Visual Studio پیدا کنید.

در Unix، استاندارد POSIX نام‌های constants را تعریف می‌کند، اما پیاده‌سازی‌های فردی سیستم‌های Unix سازگار با POSIX ممکن است مقادیر عددی متفاوتی برای این constants اختصاص دهند. باید از مقدار عددی صحیح برای سیستم‌عامل خود استفاده کنید. همچنین، POSIX یک استاندارد برای structهای استفاده‌شده در فراخوانی interop تعریف می‌کند. ترتیب فیلدها در struct توسط استاندارد ثابت نشده و ممکن است پیاده‌سازی Unix فیلدهای اضافی اضافه کند. فایل‌های هدر C++ که توابع و انواع را تعریف می‌کنند معمولاً در `/usr/include` یا `/usr/local/include` نصب می‌شوند.

---

دریافت رشته‌ها از کد غیرمدیریت‌شده به .NET نیازمند مدیریت حافظه است. marshaler به طور خودکار این کار را انجام می‌دهد اگر متد خارجی را با `StringBuilder` به جای `string` اعلام کنید، مانند:

```csharp
StringBuilder s = new StringBuilder(256);
GetWindowsDirectory(s, 256);
Console.WriteLine(s);

[DllImport("kernel32.dll")]
static extern int GetWindowsDirectory(StringBuilder sb, int maxChars);
```

در Unix نیز مشابه عمل می‌کند. مثال زیر تابع `getcwd` را فراخوانی می‌کند تا مسیر جاری را بازگرداند:

```csharp
var sb = new StringBuilder(256);
Console.WriteLine(getcwd(sb, sb.Capacity));

[DllImport("libc")]
static extern string getcwd(StringBuilder buf, int size);
```

اگرچه استفاده از `StringBuilder` راحت است، اما کمی ناکارآمد است زیرا CLR باید تخصیص‌های حافظه اضافی و کپی‌کردن‌ها را انجام دهد. در نقاط حساس عملکرد، می‌توانید با استفاده از `char[]` این سربار را کاهش دهید:

```csharp
[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
static extern int GetWindowsDirectory(char[] buffer, int maxChars);
```

توجه کنید که باید `CharSet` را در attribute `DllImport` مشخص کنید. همچنین پس از فراخوانی تابع، باید رشته خروجی را به طول مناسب برش دهید. می‌توانید این کار را با حداقل تخصیص حافظه با استفاده از **array pooling** (صفحه ۵۹۹) انجام دهید:

```csharp
string GetWindowsDirectory()
{
    var array = ArrayPool<char>.Shared.Rent(256);
    try
    {
        int length = GetWindowsDirectory(array, 256);
        return new string(array, 0, length).ToString();
    }
    finally { ArrayPool<char>.Shared.Return(array); }
}
```

(البته، این مثال صرفاً آموزشی است و شما می‌توانید مسیر Windows را از طریق متد داخلی `Environment.GetFolderPath` دریافت کنید.)

اگر مطمئن نیستید چگونه یک متد خاص Win32 یا Unix را فراخوانی کنید، معمولاً با جستجوی نام تابع و `DllImport` در اینترنت، نمونه‌ای پیدا خواهید کرد. برای ویندوز، سایت [http://www.pinvoke.net](http://www.pinvoke.net) یک ویکی است که هدف آن مستندسازی تمام signatureهای Win32 است.
 

