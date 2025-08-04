# فصل چهارم: سی شارپ پیشرفته

در این فصل، مباحث پیشرفته C# را پوشش می‌دهیم که بر مفاهیم بررسی‌شده در فصل‌های ۲ و ۳ استوار هستند. شما باید چهار بخش اول را به ترتیب بخوانید؛ می‌توانید بقیه بخش‌ها را به هر ترتیبی بخوانید.

## دلیگیت‌ها (Delegates)
دلیگیت یک شیء است که می‌داند چگونه یک متد را فراخوانی کند.

یک نوع دلیگیت (delegate type) نوع متدی را تعریف می‌کند که نمونه‌های دلیگیت می‌توانند فراخوانی کنند. به طور خاص، آن نوع بازگشتی و انواع پارامترهای متد را تعریف می‌کند. کد زیر یک نوع دلیگیت به نام Transformer تعریف می‌کند:

```C#

delegate int Transformer (int x);
```
Transformer با هر متدی که دارای نوع بازگشتی int و یک پارامتر int باشد، سازگار است، مانند این:

```C#

int Square (int x) { return x * x; }
```
یا، به صورت خلاصه‌تر:

```C#

int Square (int x) => x * x;
```
اختصاص یک متد به یک متغیر دلیگیت، یک نمونه دلیگیت (delegate instance) ایجاد می‌کند:

```C#

Transformer t = Square;
```
می‌توانید یک نمونه دلیگیت را به همان روشی که یک متد را فراخوانی می‌کنید، فراخوانی کنید:

```C#

int answer = t(3);    // answer برابر 9 است
```
در اینجا یک مثال کامل آورده شده است:

```C#

delegate int Transformer (int x);   // اعلان نوع دلیگیت
Transformer t = Square;             // ایجاد نمونه دلیگیت
int result = t(3);                  // فراخوانی دلیگیت
Console.WriteLine (result);         // 9
int Square (int x) => x * x;
```
یک نمونه دلیگیت به معنای واقعی کلمه به عنوان نماینده (delegate) برای فراخواننده عمل می‌کند: فراخواننده دلیگیت را فراخوانی می‌کند، و سپس دلیگیت متد هدف را فراخوانی می‌کند. این غیرمستقیم بودن، فراخواننده را از متد هدف جدا می‌کند.

دستور:

```C#

Transformer t = Square;
```
شکل کوتاه‌شده‌ای از:

```C#

Transformer t = new Transformer (Square);
```
است.

عبارت t(3) شکل کوتاه‌شده‌ای از t.Invoke(3) است.

از نظر فنی، وقتی به Square بدون پرانتز یا آرگومان ارجاع می‌دهیم، یک گروه متد (method group) را مشخص می‌کنیم. اگر متد overload شده باشد، C# بر اساس امضای (signature) دلیگیتی که به آن اختصاص داده می‌شود، overload صحیح را انتخاب خواهد کرد.
دلیگیت شبیه یک callback است، که یک اصطلاح عمومی برای ساختارهایی مانند اشاره‌گرهای تابع C است.

### نوشتن متدهای Plug-in با دلیگیت‌ها
به یک متغیر دلیگیت در زمان اجرا یک متد اختصاص داده می‌شود. این برای نوشتن متدهای plug-in مفید است. در این مثال، ما یک متد کاربردی به نام Transform داریم که یک تبدیل (transform) را به هر عنصر در یک آرایه اعداد صحیح اعمال می‌کند. متد Transform دارای یک پارامتر دلیگیت است که می‌توانید از آن برای مشخص کردن یک transform plug-in استفاده کنید:

```C#



int[] values = { 1, 2, 3 };
Transform (values, Square);      // متد Square را به آن متصل می‌کند
foreach (int i in values)
 Console.Write (i + "  ");      // 1   4   9

void Transform (int[] values, Transformer t)
{
  for (int i = 0; i < values.Length; i++)
    values[i] = t (values[i]);
}
int Square (int x) => x * x;
int Cube (int x) => x * x * x;
delegate int Transformer (int x);
```
می‌توانیم تبدیل را فقط با تغییر Square به Cube در خط دوم کد تغییر دهیم.
متد Transform ما یک تابع مرتبه بالاتر (higher-order function) است زیرا تابعی است که یک تابع را به عنوان آرگومان می‌گیرد. (متدی که یک دلیگیت را برمی‌گرداند نیز یک تابع مرتبه بالاتر خواهد بود.)

### هدف‌های متد نمونه و ثابت (Instance and Static Method Targets)
متد هدف یک دلیگیت می‌تواند یک متد محلی (local)، ثابت (static)، یا نمونه (instance) باشد. کد زیر یک متد هدف ثابت را نشان می‌دهد:

```C#

Transformer t = Test.Square;
Console.WriteLine (t(10));      // 100
class Test { public static int Square (int x) => x * x; }
delegate int Transformer (int x);
```
کد زیر یک متد هدف نمونه را نشان می‌دهد:

```C#

Test test = new Test();
Transformer t = test.Square;
Console.WriteLine (t(10));      // 100
class Test { public int Square (int x) => x * x; }
delegate int Transformer (int x);

```
هنگامی که یک متد نمونه به یک شیء دلیگیت اختصاص داده می‌شود، دلیگیت علاوه بر متد، یک ارجاع به نمونه‌ای که متد به آن تعلق دارد نیز حفظ می‌کند. ویژگی Target از کلاس System.Delegate این نمونه را نمایش می‌دهد (و برای دلیگیتی که به یک متد ثابت ارجاع می‌دهد، null خواهد بود). در اینجا یک مثال آورده شده است:

```C#
MyReporter r = new MyReporter();
r.Prefix = "%Complete: ";
ProgressReporter p = r.ReportProgress;
p(99);                                 // %Complete: 99
Console.WriteLine (p.Target == r);     // True
Console.WriteLine (p.Method);          // Void ReportProgress(Int32)
r.Prefix = "";
p(99);                                 // 99
public delegate void ProgressReporter (int percentComplete);
class MyReporter
{
  public string Prefix = "";
  public void ReportProgress (int percentComplete)
    => Console.WriteLine (Prefix + percentComplete);
}


```
از آنجا که نمونه در ویژگی Target دلیگیت ذخیره می‌شود، طول عمر آن (حداقل تا زمانی که دلیگیت عمر دارد) افزایش می‌یابد.


### دلیگیت‌های چندپخشی (Multicast Delegates)
تمام نمونه‌های دلیگیت قابلیت چندپخشی (multicast) دارند. این به این معنی است که یک نمونه دلیگیت می‌تواند نه تنها به یک متد هدف، بلکه به لیستی از متدهای هدف نیز ارجاع دهد. عملگرهای + و += نمونه‌های دلیگیت را با هم ترکیب می‌کنند:

```C#

SomeDelegate d = SomeMethod1;
d += SomeMethod2;
```
خط آخر از نظر عملکردی همانند کد زیر است:

```C#

d = d + SomeMethod2;
```
فراخوانی d اکنون هر دو SomeMethod1 و SomeMethod2 را فراخوانی خواهد کرد. دلیگیت‌ها به ترتیبی که اضافه می‌شوند، فراخوانی می‌گردند.
عملگرهای - و -= عملوند دلیگیت سمت راست را از عملوند دلیگیت سمت چپ حذف می‌کنند:

```C#

d -= SomeMethod1;
```
فراخوانی d اکنون تنها باعث فراخوانی SomeMethod2 خواهد شد.

فراخوانی + یا += روی یک متغیر دلیگیت با مقدار null نیز کار می‌کند و معادل اختصاص متغیر به یک مقدار جدید است:

```C#

SomeDelegate d = null;
d += SomeMethod1;       // معادل (وقتی d برابر null است) d = SomeMethod1;
```
به طور مشابه، فراخوانی -= روی یک متغیر دلیگیت با یک هدف تطابق‌یافته، معادل اختصاص null به آن متغیر است.

دلیگیت‌ها غیرقابل تغییر (immutable) هستند، بنابراین وقتی += یا -= را فراخوانی می‌کنید، در واقع یک نمونه دلیگیت جدید ایجاد کرده و آن را به متغیر موجود اختصاص می‌دهید.

اگر یک دلیگیت چندپخشی دارای نوع بازگشتی nonvoid باشد، فراخواننده مقدار بازگشتی را از آخرین متدی که فراخوانی می‌شود دریافت می‌کند. متدهای قبلی همچنان فراخوانی می‌شوند، اما مقادیر بازگشتی آن‌ها دور ریخته می‌شود. در بیشتر سناریوهایی که از دلیگیت‌های چندپخشی استفاده می‌شود، آن‌ها دارای انواع بازگشتی void هستند، بنابراین این ظرافت مطرح نمی‌شود.

تمام انواع دلیگیت به صورت ضمنی از System.MulticastDelegate که از System.Delegate به ارث می‌برد، مشتق می‌شوند. C# عملیات +, -, +=, و -= روی یک دلیگیت را به متدهای static Combine و Remove از کلاس System.Delegate کامپایل می‌کند.

#### مثال دلیگیت چندپخشی:
فرض کنید یک متد نوشته‌اید که اجرای آن زمان زیادی می‌برد. این متد می‌تواند به طور منظم با فراخوانی یک دلیگیت، پیشرفت کار را به فراخواننده گزارش دهد. در این مثال، متد HardWork یک پارامتر دلیگیت ProgressReporter دارد که آن را برای نشان دادن پیشرفت فراخوانی می‌کند:

```C#

public delegate void ProgressReporter (int percentComplete);
public class Util
{
  public static void HardWork (ProgressReporter p)
  {
    for (int i = 0; i < 10; i++)
    {
      p (i * 10);                           // فراخوانی دلیگیت
      System.Threading.Thread.Sleep (100);  // شبیه‌سازی کار سخت
    }
  }
}
```
برای نظارت بر پیشرفت، می‌توانیم یک نمونه دلیگیت چندپخشی p ایجاد کنیم، به طوری که پیشرفت توسط دو متد مستقل نظارت شود:

```C#

ProgressReporter p = WriteProgressToConsole;
p += WriteProgressToFile;
Util.HardWork (p);

void WriteProgressToConsole (int percentComplete)
  => Console.WriteLine (percentComplete);
void WriteProgressToFile (int percentComplete)
  => System.IO.File.WriteAllText ("progress.txt",
                                   percentComplete.ToString());
```
### انواع دلیگیت جنریک (Generic Delegate Types)
یک نوع دلیگیت می‌تواند شامل پارامترهای نوع جنریک باشد:

```C#

public delegate T Transformer (T arg);
```
با این تعریف، می‌توانیم یک متد کاربردی Transform عمومی بنویسیم که بر روی هر نوعی کار می‌کند:

```C#
int[] values = { 1, 2, 3 };
Util.Transform (values, Square);      // متصل کردن Square
foreach (int i in values)
  Console.Write (i + "  ");           // 1   4   9

int Square (int x) => x * x;
public class Util
{
  public static void Transform (T[] values, Transformer t)
  {
    for (int i = 0; i < values.Length; i++)
      values[i] = t (values[i]);
  }
}

```

### دلیگیت‌های Func و Action
با دلیگیت‌های جنریک، امکان نوشتن مجموعه‌ای کوچک از انواع دلیگیت‌ها فراهم می‌شود که به قدری عمومی هستند که می‌توانند برای متدهایی با هر نوع بازگشتی و هر تعداد آرگومان (منطقی) کار کنند. این دلیگیت‌ها، دلیگیت‌های Func و Action هستند که در فضای نام System تعریف شده‌اند (حاشیه‌نویسی‌های in و out نشان‌دهنده variance هستند، که به زودی در زمینه دلیگیت‌ها پوشش می‌دهیم):

```C#

delegate TResult Func                 ();
delegate TResult Func           (T arg);
delegate TResult Func   (T1 arg1, T2 arg2);
... و همین‌طور تا T16

delegate void Action                 ();
delegate void Action           (T arg);
delegate void Action   (T1 arg1, T2 arg2);

... و همین‌طور تا T16
```
این دلیگیت‌ها بسیار عمومی هستند. دلیگیت Transformer در مثال قبلی ما می‌تواند با یک دلیگیت Func جایگزین شود که یک آرگومان از نوع T می‌گیرد و یک مقدار از همان نوع را برمی‌گرداند:

```C#

public static void Transform (T[] values, Func transformer)
{
  for (int i = 0; i < values.Length; i++)
    values[i] = transformer (values[i]);
}
```
تنها سناریوهای عملی که توسط این دلیگیت‌ها پوشش داده نمی‌شوند، پارامترهای ref/out و اشاره‌گر هستند.

هنگامی که C# برای اولین بار معرفی شد، دلیگیت‌های Func و Action وجود نداشتند (زیرا جنریک‌ها وجود نداشتند). به همین دلیل تاریخی است که بسیاری از بخش‌های .NET به جای Func و Action از انواع دلیگیت‌های سفارشی استفاده می‌کنند.
