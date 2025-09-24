# فصل بیست و یکم:  Threading پیشرفته 

ما در **فصل ۱۴** با مبانی اولیه‌ی **Threading** شروع کردیم تا مقدمه‌ای برای **Tasks** و **Asynchrony** باشد. به طور مشخص، نشان دادیم چطور می‌توان یک **Thread** را شروع و پیکربندی کرد و مفاهیم اساسی مثل **Thread Pooling**، **Blocking**، **Spinning** و **Synchronization Contexts** را پوشش دادیم. همچنین به **Locking** و **Thread Safety** پرداختیم و ساده‌ترین سازه‌ی سیگنال‌دهی، یعنی **ManualResetEvent** را معرفی کردیم.

این فصل دقیقاً از همان‌جایی ادامه پیدا می‌کند که فصل ۱۴ در موضوع **Threading** متوقف شد. در سه بخش اول، به‌طور عمیق‌تر به **Synchronization**، **Locking** و **Thread Safety** می‌پردازیم. سپس موارد زیر را پوشش می‌دهیم:

* 🔓 **Nonexclusive locking** (مانند **Semaphore** و **Reader/Writer Locks**)
* 🔔 همه‌ی سازه‌های سیگنال‌دهی (**AutoResetEvent**، **ManualResetEvent**، **CountdownEvent** و **Barrier**)
* 🐢 **Lazy Initialization** (با استفاده از **Lazy<T>** و **LazyInitializer**)
* 🧩 **Thread-local storage** (مانند **ThreadStaticAttribute**، **ThreadLocal<T>** و **GetData/SetData**)
* ⏱ **Timers**

موضوع **Threading** آن‌قدر وسیع است که ما بخش‌های تکمیلی را به‌صورت آنلاین قرار داده‌ایم تا تصویر کامل‌تری ارائه شود. برای مطالعه‌ی موضوعات پیشرفته‌تر، به وب‌سایت زیر مراجعه کنید:

🔗 [http://albahari.com/threading](http://albahari.com/threading)

در آن‌جا مباحث زیر را خواهید یافت:

* ⚙️ **Monitor.Wait** و **Monitor.Pulse** برای سناریوهای خاص سیگنال‌دهی
* 🚀 تکنیک‌های **Nonblocking Synchronization** برای **Micro-Optimization** (مانند **Interlocked**، **Memory Barriers**، **volatile**)
* 🔄 **SpinLock** و **SpinWait** برای سناریوهای با **Concurrency** بالا

---

## 🔑 Synchronization Overview (مرور Synchronization)

**Synchronization** یعنی هماهنگ‌سازی عملیات همزمان برای دستیابی به یک نتیجه‌ی قابل پیش‌بینی. این موضوع به‌ویژه وقتی اهمیت پیدا می‌کند که چندین **Thread** به داده‌ی مشترک دسترسی دارند؛ در این شرایط، خیلی راحت می‌توان دچار مشکل شد.

ساده‌ترین و کاربردی‌ترین ابزارهای **Synchronization** احتمالاً همان **Continuations** و **Task Combinators** هستند که در فصل ۱۴ توضیح دادیم. با فرموله کردن برنامه‌های همزمان در قالب عملیات **Asynchronous** که با **Continuations** و **Combinators** به هم متصل می‌شوند، نیاز به **Locking** و **Signaling** کمتر می‌شود. با این حال، هنوز مواقعی وجود دارد که سازه‌های سطح پایین‌تر وارد عمل می‌شوند.

سازه‌های **Synchronization** را می‌توان به سه دسته تقسیم کرد:

1. 🔒 **Exclusive Locking**
   این سازه‌ها اجازه می‌دهند فقط یک **Thread** در هر لحظه فعالیت خاصی انجام دهد یا بخشی از کد را اجرا کند. هدف اصلی آن‌ها این است که **Threads** بتوانند به وضعیت مشترک در حال نوشتن دسترسی داشته باشند بدون این‌که مزاحم هم شوند. سازه‌های **Exclusive Locking** عبارت‌اند از:
   **lock**، **Mutex** و **SpinLock**.

2. 🔓 **Nonexclusive Locking**
   این نوع **Locking** به شما اجازه می‌دهد میزان **Concurrency** را محدود کنید. سازه‌های آن عبارت‌اند از:
   **Semaphore(Slim)** و **ReaderWriterLock(Slim)**.

3. 📢 **Signaling**
   این سازه‌ها به یک **Thread** اجازه می‌دهند تا زمانی که از یک یا چند **Thread** دیگر اطلاع (Notification) دریافت نکرده، در حالت **Block** باقی بماند. سازه‌های **Signaling** شامل **ManualResetEvent(Slim)**، **AutoResetEvent**، **CountdownEvent** و **Barrier** هستند. سه مورد اول معمولاً با عنوان **Event Wait Handles** شناخته می‌شوند.

همچنین امکان (و البته دشواری) انجام برخی عملیات همزمان روی داده‌ی مشترک بدون استفاده از **Locking** وجود دارد، با کمک سازه‌های **Nonblocking Synchronization**. این سازه‌ها عبارت‌اند از:
**Thread.MemoryBarrier**، **Thread.VolatileRead**، **Thread.VolatileWrite**، کلیدواژه‌ی **volatile** و کلاس **Interlocked**.

ما این موضوع را به همراه متدهای **Monitor.Wait/Pulse** که برای نوشتن منطق سفارشی سیگنال‌دهی کاربرد دارند، به‌صورت آنلاین پوشش داده‌ایم.

---

## 🔐 Exclusive Locking (قفل‌گذاری انحصاری)

سه سازه‌ی اصلی برای **Exclusive Locking** وجود دارد: دستور **lock**، **Mutex** و **SpinLock**.

* دستور **lock** راحت‌ترین و پرکاربردترین گزینه است.
* **Mutex** زمانی استفاده می‌شود که بخواهید قفل را بین چندین **Process** گسترش دهید (قفل‌های سطح کامپیوتر).
* **SpinLock** یک **Micro-Optimization** است که می‌تواند در سناریوهای با **Concurrency** بالا باعث کاهش **Context Switches** شود. 🔄


### 🔒 The `lock` Statement (دستور lock)

برای نشان دادن نیاز به **Locking**، کلاس زیر را در نظر بگیرید:

```csharp
class ThreadUnsafe
{
  static int _val1 = 1, _val2 = 1;
  static void Go()
  {
    if (_val2 != 0) Console.WriteLine (_val1 / _val2);
    _val2 = 0;
  }
}
```

این کلاس **Thread-Safe** نیست: اگر متد `Go` به‌طور همزمان توسط دو **Thread** فراخوانی شود، امکان رخ دادن خطای **Division by Zero** وجود دارد. چرا؟ چون ممکن است در همان لحظه‌ای که یک **Thread** بین اجرای دستور `if` و `Console.WriteLine` است، **Thread** دیگر مقدار `_val2` را برابر صفر قرار دهد.

اینجاست که دستور **lock** مشکل را حل می‌کند:

```csharp
class ThreadSafe
{
  static readonly object _locker = new object();
  static int _val1 = 1, _val2 = 1;
  static void Go()
  {
    lock (_locker)
    {
      if (_val2 != 0) Console.WriteLine (_val1 / _val2);
      _val2 = 0;
    }
  }
}
```

فقط یک **Thread** در هر لحظه می‌تواند شیء همگام‌ساز (در اینجا `_locker`) را قفل کند. هر **Thread** دیگری که برای قفل رقابت کند، **Blocked** می‌شود تا زمانی که قفل آزاد شود.

اگر بیش از یک **Thread** برای قفل رقابت کند، آن‌ها در یک **Ready Queue** قرار می‌گیرند و به ترتیب ورود، قفل به آن‌ها داده می‌شود (البته ✍️ در بعضی شرایط سیستم‌عامل **Windows** و **CLR** ممکن است این عدالت نقض شود).

به همین دلیل، قفل‌های انحصاری را گاهی اوقات طوری توصیف می‌کنند که **دسترسی سریالی** را به چیزی که توسط قفل محافظت می‌شود، اعمال می‌کنند؛ زیرا دسترسی یک **Thread** نمی‌تواند با دیگری همپوشانی داشته باشد. در این مثال، ما هم منطق داخل متد `Go` و هم فیلدهای `_val1` و `_val2` را محافظت کرده‌ایم.

---

### ⚙️ Monitor.Enter و Monitor.Exit

دستور `lock` در C# در حقیقت یک **میان‌بر نحوی (Syntactic Shortcut)** برای فراخوانی متدهای `Monitor.Enter` و `Monitor.Exit` به همراه یک بلوک `try/finally` است.

به‌طور ساده، کدی که در متد `Go` اتفاق می‌افتد، معادل زیر است:

```csharp
Monitor.Enter (_locker);
try
{
  if (_val2 != 0) Console.WriteLine (_val1 / _val2);
  _val2 = 0;
}
finally { Monitor.Exit (_locker); }
```

⚠️ اگر متد `Monitor.Exit` بدون این‌که قبلاً `Monitor.Enter` روی همان شیء صدا زده شده باشد، فراخوانی شود، یک **Exception** پرتاب می‌شود.

---

### 🛡 overloadهای lockTaken

کدی که در بالا دیدیم یک آسیب‌پذیری ظریف دارد. فرض کنید (هرچند بعید) یک **Exception** بین فراخوانی `Monitor.Enter` و شروع بلوک `try` رخ دهد (مثلاً یک **OutOfMemoryException** یا در **.NET Framework** اگر **Thread** متوقف شود).

در این شرایط:

* اگر قفل گرفته نشده باشد، خطری وجود ندارد.
* اما اگر قفل گرفته شده باشد، چون هیچ‌وقت وارد بلوک `try/finally` نمی‌شویم، قفل آزاد نخواهد شد. این یعنی **Leak شدن قفل**.

برای جلوگیری از این مشکل، متد زیر در **Monitor.Enter** تعریف شده است:

```csharp
public static void Enter (object obj, ref bool lockTaken);
```

🔎 اگر و فقط اگر متد `Enter` یک **Exception** پرتاب کند و قفل گرفته نشده باشد، مقدار `lockTaken` برابر **false** خواهد بود.

الگوی درست استفاده از آن (و همان چیزی که کامپایلر C# در پشت‌صحنه برای دستور `lock` تولید می‌کند) به شکل زیر است:

```csharp
bool lockTaken = false;
try
{
  Monitor.Enter (_locker, ref lockTaken);
  // Do your stuff...
}
finally { if (lockTaken) Monitor.Exit (_locker); }
```

---

### ⏱ TryEnter

کلاس **Monitor** همچنین متدی به نام **TryEnter** ارائه می‌دهد که به شما اجازه می‌دهد یک **Timeout** مشخص کنید (بر حسب **Milliseconds** یا یک **TimeSpan**).

* اگر قفل گرفته شود، مقدار **true** برمی‌گرداند.
* اگر قفل به‌دلیل **Timeout** گرفته نشود، مقدار **false** برمی‌گرداند.

علاوه بر این، متد **TryEnter** می‌تواند بدون هیچ آرگومانی فراخوانی شود. در این حالت، فقط **قفل را تست می‌کند** و اگر بلافاصله نتواند قفل را بگیرد، بدون معطلی **false** برمی‌گرداند.

📌 درست مثل متد `Enter`، متد **TryEnter** هم یک نسخه‌ی overload دارد که از آرگومان **lockTaken** پشتیبانی می‌کند.

### 🔑 Choosing the Synchronization Object (انتخاب شیء همگام‌ساز)

شما می‌توانید از هر شیئی که برای **Thread**‌های شرکت‌کننده قابل مشاهده باشد، به‌عنوان شیء همگام‌ساز استفاده کنید؛ با این شرط مهم که آن شیء باید یک **Reference Type** باشد.

شیء همگام‌ساز معمولاً **private** است (چون این کار باعث می‌شود منطق قفل‌گذاری بهتر **Encapsulate** شود) و معمولاً یک **Instance Field** یا یک **Static Field** است.

گاهی اوقات شیء همگام‌ساز همان شیء محافظت‌شده است. مانند فیلد `_list` در مثال زیر:

```csharp
class ThreadSafe
{
  List<string> _list = new List<string>();
  void Test()
  {
    lock (_list)
    {
      _list.Add("Item 1");
      ...
    }
  }
}
```

البته داشتن یک فیلد اختصاصی برای قفل‌گذاری (مثل `_locker` در مثال قبلی) کنترل دقیق‌تری روی **Scope** و **Granularity** قفل فراهم می‌کند.

همچنین می‌توانید از شیء حاوی (یعنی `this`) به‌عنوان شیء همگام‌ساز استفاده کنید:

```csharp
lock (this) { ... }
```

یا حتی از نوع کلاس استفاده کنید:

```csharp
lock (typeof(Widget)) { ... }   // برای محافظت از فیلدهای static
```

❌ عیب این روش‌ها این است که منطق قفل‌گذاری **Encapsulate** نمی‌شود و همین می‌تواند مدیریت **Deadlock** و **Blocking** بیش از حد را سخت‌تر کند.

شما حتی می‌توانید روی متغیرهای محلی که توسط **Lambda Expressions** یا **Anonymous Methods** گرفته شده‌اند نیز قفل بگذارید.

---

### ⏰ When to Lock (چه زمانی باید قفل کنیم)

قفل کردن دسترسی به خود شیء همگام‌ساز را محدود نمی‌کند. به عبارت دیگر، اگر متدی مثل `x.ToString()` فراخوانی شود، **Blocked** نخواهد شد فقط به این دلیل که یک **Thread** دیگر روی `lock(x)` قفل کرده است.
فقط زمانی **Blocking** اتفاق می‌افتد که هر دو **Thread** از `lock(x)` استفاده کنند.

قاعده‌ی پایه‌ای این است:
🔒 شما باید همیشه هنگام دسترسی به هر **Shared Writable Field** از قفل استفاده کنید.

حتی در ساده‌ترین حالت—مثلاً یک عمل **Assignment** روی یک فیلد—باید **Synchronization** را در نظر بگیرید.

به مثال زیر توجه کنید:

```csharp
class ThreadUnsafe
{
  static int _x;
  static void Increment() { _x++; }
  static void Assign()    { _x = 123; }
}
```

این کلاس **Thread-Safe** نیست. نسخه‌ی ایمن‌تر آن به شکل زیر است:

```csharp
static readonly object _locker = new object();
static int _x;

static void Increment() { lock (_locker) _x++; }
static void Assign()    { lock (_locker) _x = 123; }
```

---

### ⚠️ مشکلات بدون Lock

اگر قفل وجود نداشته باشد، دو مشکل رخ می‌دهد:

1. ➕ عملیات‌هایی مثل افزایش مقدار یک متغیر (یا حتی خواندن/نوشتن آن در شرایط خاص) **Atomic** نیستند.
2. ⚡ **Compiler**، **CLR** و **Processor** مجاز هستند برای بهبود کارایی، دستورات را **Reorder** کنند یا متغیرها را در **CPU Registers** کش کنند—تا زمانی که این بهینه‌سازی‌ها رفتار یک برنامه‌ی تک‌ریسمانی (یا چندریسمانی که از قفل استفاده می‌کند) را تغییر ندهد.

قفل‌ها مشکل دوم را با ایجاد یک **Memory Barrier** قبل و بعد از قفل کاهش می‌دهند.

🧱 **Memory Barrier** مانند یک حصار است که مانع عبور اثرات **Reordering** و **Caching** می‌شود.

این قاعده فقط مخصوص قفل‌ها نیست؛ بلکه برای همه‌ی سازه‌های **Synchronization** صدق می‌کند.

مثال: اگر یک سازه‌ی سیگنال‌دهی تضمین کند که فقط یک **Thread** در هر لحظه متغیری را بخواند/بنویسد، دیگر نیازی به قفل ندارید:

```csharp
var signal = new ManualResetEvent(false);
int x = 0;
new Thread(() => { x++; signal.Set(); }).Start();
signal.WaitOne();
Console.WriteLine(x);   // همیشه 1
```

در بخش «**Nonblocking Synchronization**» توضیح داده‌ایم که چرا چنین نیازی پیش می‌آید و چگونه **Memory Barriers** و کلاس **Interlocked** می‌توانند جایگزین قفل در این سناریوها باشند.

---

### 🔗 Locking and Atomicity (قفل‌گذاری و اتمیک بودن)

اگر گروهی از متغیرها همیشه درون یک قفل خوانده یا نوشته شوند، می‌توان گفت این متغیرها به شکل **Atomic** دسترسی دارند.

مثال:

```csharp
lock (locker) { if (x != 0) y /= x; }
```

در این حالت، متغیرهای `x` و `y` به‌صورت **Atomic** دسترسی داده می‌شوند؛ یعنی هیچ **Thread** دیگری نمی‌تواند در میانه‌ی این عملیات آن‌ها را تغییر دهد و نتیجه را بی‌اعتبار کند. بنابراین، شما هرگز خطای **Division by Zero** دریافت نمی‌کنید، مشروط بر این‌که `x` و `y` همیشه در همین قفل انحصاری دسترسی داده شوند.

---

### 🚨 استثناها و Atomicity

اتمیک بودن قفل نقض می‌شود اگر داخل بلوک قفل یک **Exception** رخ دهد (چه برنامه چندریسمانی باشد چه نباشد).

مثال:

```csharp
decimal _savingsBalance, _checkBalance;
void Transfer(decimal amount)
{
  lock (_locker)
  {
    _savingsBalance += amount;
    _checkBalance -= amount + GetBankFee();
  }
}
```

اگر متد `GetBankFee()` یک **Exception** پرتاب کند، بانک پول از دست می‌دهد! 🏦💸

در این شرایط می‌توان مشکل را با فراخوانی `GetBankFee` پیش از ورود به بلوک قفل برطرف کرد.

🔄 برای موارد پیچیده‌تر، یک راه‌حل این است که منطق **Rollback** را داخل یک بلوک `catch` یا `finally` پیاده‌سازی کنیم.

---

### ⚡ Instruction Atomicity

مفهوم **Instruction Atomicity** متفاوت اما مشابه است: یک دستور **Atomic** است اگر روی پردازنده‌ی زیرین به‌طور غیرقابل تقسیم اجرا شود.

### 🔐 قفل‌های تو در تو (Nested Locking)

یک **Thread** می‌تواند بارها یک شیء را به‌صورت تو در تو (یا **Reentrant**) قفل کند:

```csharp
lock (locker)
  lock (locker)
    lock (locker)
    {
       // Do something...
    }
```

یا به روش دیگر:

```csharp
Monitor.Enter (locker); 
Monitor.Enter (locker);  
Monitor.Enter (locker); 

// Do something...

Monitor.Exit (locker);  
Monitor.Exit (locker);   
Monitor.Exit (locker);
```

در این حالت‌ها، شیء تنها زمانی **آزاد (unlock)** می‌شود که یا خارجی‌ترین دستور `lock` پایان یافته باشد، یا تعداد متناظری از `Monitor.Exit` اجرا شده باشد.

🔁 **قفل تو در تو** زمانی مفید است که یک متد از داخل یک قفل، متد دیگری را صدا بزند:

```csharp
object locker = new object();

lock (locker)
{
    AnotherMethod();
    // هنوز قفل داریم - چون قفل‌ها Reentrant هستند
}

void AnotherMethod()
{
    lock (locker) { Console.WriteLine ("Another method"); }
}
```

✅ در اینجا Thread تنها روی اولین (خارجی‌ترین) قفل مسدود می‌شود.

---

### ⚠️ بن‌بست‌ها (Deadlocks)

**Deadlock** زمانی اتفاق می‌افتد که دو Thread هرکدام منتظر یک منبع باشند که توسط دیگری قفل شده است؛ در نتیجه هیچ‌کدام قادر به ادامه کار نخواهند بود.

مثال ساده با دو قفل:

```csharp
object locker1 = new object();
object locker2 = new object();

new Thread (() => {
    lock (locker1)
    {
        Thread.Sleep (1000);
        lock (locker2);   // Deadlock
    }
}).Start();

lock (locker2)
{
    Thread.Sleep (1000);
    lock (locker1);       // Deadlock
}
```

📌 در این حالت، هر Thread یکی از قفل‌ها را گرفته و منتظر دیگری است → **بن‌بست دائمی**.

* در محیط عادی CLR، بر خلاف SQL Server، **بن‌بست‌ها به‌صورت خودکار تشخیص و رفع نمی‌شوند**.
* اگر بن‌بست رخ دهد، Threadها به‌طور نامحدود مسدود می‌شوند (مگر این‌که زمان‌انتظار یا Timeout تعریف کرده باشید).
* در هاست SQL CLR (ادغام SQL Server با CLR)، بن‌بست‌ها شناسایی می‌شوند و یک استثنای قابل‌مدیریت روی یکی از Threadها پرتاب می‌شود.

---

### 🔎 چرا Deadlock سخت است؟

* هنگام طراحی شی‌ءگرا، ممکن است قفل‌ها در زنجیره‌ای از متدهای تو در تو گرفته شوند که ترتیب آن‌ها در زمان اجرا مشخص می‌شود.
* ممکن است شما روی فیلد `a` در کلاس `x` قفل بزنید، در حالی که فراخواننده‌ی شما قبلاً روی فیلد `b` در کلاس `y` قفل زده باشد. هم‌زمان Thread دیگری برعکس همین کار را انجام دهد → Deadlock.
* الگوهای خوب طراحی شی‌ءگرا (OOP) این مشکل را تشدید می‌کنند چون فراخوانی‌ها در زمان اجرا تعیین می‌شوند.

---

### ✅ راهکارها برای کاهش Deadlock

1. **قفل‌ها را به ترتیب ثابت بگیرید** (روش کلاسیک، ولی همیشه عملی نیست).
2. هنگام قفل کردن اطراف فراخوانی متدهای دیگر احتیاط کنید.
3. بررسی کنید آیا واقعاً لازم است هنگام فراخوانی متدهای دیگر قفل بگیرید یا خیر.
4. استفاده بیشتر از **سینکرون‌سازی سطح بالاتر** مثل:

   * Task continuations/combinators
   * Data parallelism
   * Immutable types

💡 نکته: زمانی که در حین داشتن یک قفل، کدی بیرونی را صدا می‌زنید، **کپسوله‌سازی قفل نشت می‌کند**. این یک محدودیت ذاتی مکانیزم قفل‌گذاری است، نه یک ضعف CLR.

---

### ⚡️ سناریوی خاص Deadlock در UI

* در اپلیکیشن‌های WPF: `Dispatcher.Invoke`
* در Windows Forms: `Control.Invoke`

اگر این‌ها را هنگام داشتن یک قفل صدا بزنید و Thread UI منتظر همان قفل باشد → Deadlock.

راه‌حل‌ها:

* استفاده از `BeginInvoke` به‌جای `Invoke`.
* یا آزاد کردن قفل قبل از صدا زدن `Invoke` (البته اگر قفل توسط Caller گرفته شده باشد، جواب نمی‌دهد).

---

### 🚀 کارایی (Performance)

* گرفتن و آزاد کردن قفل در حالت **بدون رقابت (uncontended)** روی سیستم‌های 2020 کمتر از **۲۰ نانوثانیه** طول می‌کشد.
* در حالت **رقابتی (contended)**، هزینه به حدود **میکروثانیه** می‌رسد (به‌خاطر Context Switch).
* زمان واقعی باززمان‌بندی Thread ممکن است حتی بیشتر طول بکشد.

### 🔒 **Mutex (موتکس)**

یک **Mutex** شبیه به دستور `lock` در C# است، با این تفاوت که می‌تواند در **چند پردازه (multi-process)** هم کار کند. یعنی:

* `lock` فقط داخل یک پردازه کاربرد دارد.
* `Mutex` هم **سراسری در سطح کامپیوتر (computer-wide)** و هم **سطح برنامه (application-wide)** قابل استفاده است.

⏱ گرفتن و آزاد کردن یک Mutex (در حالت بدون رقابت) حدود **۰.۵ میکروثانیه** طول می‌کشد؛ یعنی بیش از **۲۰ برابر کندتر از lock**.

---

### 📌 استفاده از Mutex

* برای گرفتن قفل: `WaitOne()`
* برای آزاد کردن: `ReleaseMutex()`
* دقیقاً مثل `lock`، فقط همان **Thread** که Mutex را گرفته، می‌تواند آن را آزاد کند.

⚠️ اگر `ReleaseMutex` را فراموش کنید و فقط `Close` یا `Dispose` صدا بزنید،
اولین Thread دیگری که روی آن منتظر مانده باشد، با **AbandonedMutexException** مواجه می‌شود.

---

### 🎯 کاربرد متداول: محدود کردن اجرای چندین نسخه از یک برنامه

مثال:

```csharp
// با نام‌گذاری Mutex، آن را در سطح کل کامپیوتر قابل‌دسترس می‌کنیم.
// بهتر است نام، یکتا باشد (مثلاً شامل نام شرکت یا URL).
using var mutex = new Mutex (true, @"Global\oreilly.com OneAtATimeDemo");

// اگر قفل اشغال بود، چند ثانیه صبر کنیم
// شاید نسخه قبلی برنامه در حال بستن باشد.
if (!mutex.WaitOne (TimeSpan.FromSeconds (3), false))
{
    Console.WriteLine ("Another instance of the app is running. Bye!");
    return;
}

try { RunProgram(); }
finally { mutex.ReleaseMutex(); }

void RunProgram()
{
    Console.WriteLine ("Running. Press Enter to exit");
    Console.ReadLine();
}
```

📌 نکته:

* در **Terminal Services** یا **کنسول‌های یونیکس جداگانه**، Mutex سراسری معمولاً فقط برای برنامه‌هایی در همان **Session** قابل‌دسترسی است.
* اگر بخواهید برای همه‌ی Sessionها در دسترس باشد، باید نام Mutex را با `"Global\"` شروع کنید (مثل نمونه کد).

---

### 🧩 Thread Safety (ایمنی در چندنخی)

یک برنامه یا متد زمانی **Thread-Safe** است که در هر سناریوی چندنخی درست کار کند.
این کار عمدتاً با **قفل‌گذاری (locking)** و **کاهش تعامل Threadها** به دست می‌آید.

#### چرا همه‌چیز همیشه Thread-Safe نیست؟

1. سربار توسعه زیاد است (اگر یک نوع تعداد زیادی فیلد داشته باشد، هر فیلد می‌تواند منبع تداخل باشد).
2. Thread-Safety هزینه‌ی کارایی دارد (حتی اگر فقط یک Thread استفاده کند).
3. حتی اگر یک کلاس Thread-Safe باشد، **برنامه‌ای که از آن استفاده می‌کند، لزوماً Thread-Safe نخواهد بود**.

➡️ بنابراین، معمولاً **فقط همان‌جایی Thread-Safe پیاده‌سازی می‌شود که لازم است**.

---

### ⚙️ راه‌های "میان‌بر" برای Thread-Safety

1. **قفل‌گذاری کلی (Coarse-Grained Locking)**

   * کل دسترسی به یک شیء را داخل یک قفل انحصاری قرار دهید.
   * این روش به‌خصوص برای استفاده از **کدهای ناامن در چندنخی** (مثل بیشتر انواع .NET یا کدهای شخص ثالث) ضروری است.
   * اگر متدهای شیء سریع باشند، این روش جواب می‌دهد. (وگرنه قفل زیاد باعث بلوکه شدن خواهد شد).

2. **کاهش تعامل Threadها با کاهش داده‌های مشترک**

   * این استراتژی در اپلیکیشن‌های Stateless (مثل وب‌سرورها یا سرویس‌های میانی) استفاده می‌شود.
   * چون داده بین درخواست‌ها ذخیره نمی‌شود، تعامل Threadها کم می‌شود.
   * تنها نقاط مشترک می‌توانند فیلدهای `static` باشند (مثلاً برای کش کردن داده یا سرویس‌های زیرساختی مثل Authentication و Auditing).

3. **اجرای کد دسترسی به داده‌های مشترک روی Thread رابط کاربری (UI Thread)**

   * در اپلیکیشن‌های کلاینت غنی (Rich Client).
   * با استفاده از **توابع Asynchronous** (که در فصل 14 دیدیم) این کار راحت‌تر می‌شود.

---

📖 جمع‌بندی:

* از **Mutex** وقتی استفاده کنید که نیاز به هماهنگی بین چند پردازه دارید (مثل اطمینان از اجرای تنها یک نسخه‌ی برنامه).
* برای **Thread-Safety**، یا باید با قفل‌ها دسترسی را کنترل کنید، یا با طراحی صحیح (مثل Stateless یا Immutable) تعامل Threadها را به حداقل برسانید.

### ایمنی نخ‌ها و انواع .NET 🧵🛡️

شما می‌توانید با استفاده از **locking** کدی که thread-unsafe است را به کدی thread-safe تبدیل کنید. یکی از کاربردهای خوب این کار در **.NET** است: تقریباً همه‌ی انواع **nonprimitive** (غیرابتدایی) در .NET زمانی که ساخته می‌شوند، thread-safe نیستند (برای چیزی بیشتر از دسترسی فقط‌خواندنی). با این حال، شما می‌توانید آن‌ها را در کد چندنخی (multithreaded) استفاده کنید، به شرطی که همه‌ی دسترسی‌ها به یک شیء مشخص با استفاده از یک **lock** محافظت شوند.

در اینجا مثالی داریم که در آن دو نخ به‌طور همزمان یک آیتم را به همان **List collection** اضافه می‌کنند و سپس آن لیست را پیمایش می‌کنند:

```csharp
class ThreadSafe
{
  static List<string> _list = new List<string>();
  static void Main()
  {
    new Thread (AddItem).Start();
    new Thread (AddItem).Start();
  }
  static void AddItem()
  {
    lock (_list) _list.Add ("Item " + _list.Count);
    string[] items;
    lock (_list) items = _list.ToArray();
    foreach (string s in items) Console.WriteLine (s);
  }
}
```

---

### قفل‌گذاری و ایمنی نخ‌ها 🔐

در این حالت، ما روی خود شیء **\_list** قفل می‌کنیم. اگر دو لیست مرتبط داشتیم، باید یک شیء مشترک را برای قفل انتخاب می‌کردیم (می‌توانستیم یکی از لیست‌ها را انتخاب کنیم یا بهتر: از یک فیلد مستقل استفاده کنیم).

پیمایش (enumerating) مجموعه‌های .NET نیز thread-unsafe است، به این معنا که اگر لیست هنگام پیمایش تغییر کند، یک **exception** رخ می‌دهد. به جای قفل‌گذاری در کل مدت پیمایش، در این مثال ابتدا آیتم‌ها را در یک **آرایه** کپی می‌کنیم. این کار باعث می‌شود قفل برای مدت طولانی نگه داشته نشود، مخصوصاً اگر کاری که هنگام پیمایش انجام می‌دهیم زمان‌بر باشد. (راه‌حل دیگر استفاده از یک **reader/writer lock** است؛ به بخش «Reader/Writer Locks» در صفحه ۹۰۷ مراجعه کنید.)

---

### قفل‌گذاری روی اشیاء thread-safe ⚡

گاهی لازم است حتی هنگام دسترسی به اشیاء **thread-safe** نیز از قفل استفاده کنید. برای توضیح، فرض کنید که کلاس **List** در .NET واقعاً thread-safe بود و ما می‌خواستیم یک آیتم به لیست اضافه کنیم:

```csharp
if (!_list.Contains (newItem)) _list.Add (newItem);
```

صرف‌نظر از اینکه لیست thread-safe باشد یا نه، این دستور قطعاً thread-safe نیست! کل عبارت **if** باید درون یک قفل قرار گیرد تا از پیش‌دستی (preemption) بین بررسی عضویت و اضافه کردن آیتم جلوگیری شود. این قفل باید در همه‌ی جاهایی که لیست را تغییر می‌دهیم استفاده شود. برای مثال، دستور زیر نیز باید در همان قفل پیچیده شود تا از پیش‌دستی نسبت به عبارت قبلی جلوگیری کند:

```csharp
_list.Clear();
```

به عبارت دیگر، باید دقیقاً همانند کلاس‌های مجموعه‌ی thread-unsafe قفل‌گذاری کنیم (که این موضوع ایمنی نخِ فرضیِ کلاس **List** را بی‌اثر می‌سازد).

---

### اعضای ایستا (Static Members) ⚙️

قفل‌گذاری هنگام دسترسی به یک مجموعه می‌تواند در محیط‌های با همروندی بالا (**highly concurrent environments**) باعث **blocking** بیش‌ازحد شود. برای همین، .NET یک **queue**، **stack** و **dictionary** thread-safe فراهم کرده است که در فصل ۲۲ بررسی خواهیم کرد.

پیچیدن دسترسی به یک شیء در یک قفل سفارشی فقط وقتی کار می‌کند که همه‌ی نخ‌های همروند از آن قفل آگاه باشند و از آن استفاده کنند. این ممکن است وقتی شیء در سطح وسیعی استفاده می‌شود برقرار نباشد. بدترین حالت در مورد **static members** در یک نوع عمومی (public type) رخ می‌دهد.

برای مثال، تصور کنید که ویژگی ایستای **DateTime.Now** در ساختار **DateTime** thread-safe نبود و دو فراخوانی همزمان می‌توانست خروجی درهم یا یک **exception** ایجاد کند. تنها راه‌حل با قفل‌گذاری خارجی این بود که قبل از فراخوانی DateTime.Now نوع را قفل کنیم:

```csharp
lock(typeof(DateTime))
```

این فقط زمانی جواب می‌دهد که همه‌ی برنامه‌نویسان با این کار موافق باشند (که بعید است). علاوه بر این، قفل‌گذاری روی یک نوع مشکلات خودش را ایجاد می‌کند.

به همین دلیل، اعضای ایستای **DateTime struct** به‌طور دقیق thread-safe پیاده‌سازی شده‌اند. این یک الگوی رایج در .NET است:

* اعضای ایستا (static members) → **thread-safe** ✅
* اعضای نمونه (instance members) → **thread-unsafe** ❌

دنبال کردن این الگو هنگام نوشتن انواع برای استفاده عمومی منطقی است تا از ایجاد معماهای غیرممکنِ ایمنی نخ جلوگیری شود. به عبارت دیگر، با thread-safe کردن متدهای ایستا، شما طوری کدنویسی می‌کنید که مانع ایمنی نخ برای مصرف‌کنندگان آن نوع نشوید.

⚠️ **ایمنی نخ در متدهای ایستا چیزی است که باید به‌طور صریح پیاده‌سازی شود؛ این ویژگی به‌طور خودکار فقط به دلیل ایستا بودن متد اتفاق نمی‌افتد!**

---

### ایمنی نخ در حالت فقط‌خواندنی 📖

ایمن کردن انواع برای دسترسی همزمان فقط‌خواندنی (در صورت امکان) سودمند است زیرا به این معناست که مصرف‌کنندگان می‌توانند از قفل‌گذاری بیش‌ازحد جلوگیری کنند. بسیاری از انواع .NET این اصل را دنبال می‌کنند: برای مثال، مجموعه‌ها برای خوانندگان همزمان thread-safe هستند.

دنبال کردن این اصل برای خودتان ساده است: اگر نوعی را به‌عنوان thread-safe برای دسترسی همزمان فقط‌خواندنی مستند می‌کنید، در متدهایی که مصرف‌کننده انتظار دارد فقط‌خواندنی باشند به فیلدها ننویسید (یا در صورت نیاز قفل‌گذاری کنید).

برای نمونه، در پیاده‌سازی یک متد **ToArray()** در یک مجموعه، ممکن است بخواهید ابتدا ساختار داخلی مجموعه را فشرده‌سازی کنید. با این حال، این کار آن را برای مصرف‌کنندگانی که انتظار داشتند فقط‌خواندنی باشد، thread-unsafe می‌کند.

ایمنی نخ در حالت فقط‌خواندنی یکی از دلایلی است که **enumerator** ها از **enumerable** ها جدا هستند: دو نخ می‌توانند به‌طور همزمان روی یک مجموعه پیمایش کنند چون هرکدام یک شیء enumerator جدا دریافت می‌کنند.

در نبود مستندات، بهتر است محتاط باشید و فرض نکنید که یک متد ذاتاً فقط‌خواندنی است. یک مثال خوب کلاس **Random** است: وقتی **Random.Next()** را فراخوانی می‌کنید، پیاده‌سازی داخلی آن نیاز دارد که مقادیر بذر خصوصی (private seed values) را به‌روزرسانی کند. بنابراین، شما باید یا هنگام استفاده از کلاس Random قفل‌گذاری کنید یا یک نمونه جداگانه برای هر نخ نگه دارید.

### ایمنی نخ در سرورهای برنامه 🖥️🧵

سرورهای برنامه (Application servers) باید **چندنخی (multithreaded)** باشند تا بتوانند درخواست‌های همزمان کلاینت‌ها را مدیریت کنند. برنامه‌های **ASP.NET Core** و **Web API** به‌صورت ضمنی چندنخی هستند. این یعنی هنگام نوشتن کد در سمت سرور، اگر احتمال تعامل میان نخ‌هایی که درخواست‌های کلاینت را پردازش می‌کنند وجود داشته باشد، باید **ایمنی نخ (thread safety)** را در نظر بگیرید. خوشبختانه، چنین احتمالی نادر است؛ یک کلاس معمولی در سرور یا **stateless** است (هیچ فیلدی ندارد) یا یک مدل فعال‌سازی دارد که برای هر کلاینت یا هر درخواست یک نمونه‌ی جدا از شیء می‌سازد. تعامل معمولاً فقط از طریق **static fields** رخ می‌دهد، که گاهی برای کش کردن بخش‌هایی از دیتابیس در حافظه جهت بهبود کارایی استفاده می‌شوند.

برای مثال، فرض کنید متدی به نام **RetrieveUser** دارید که یک دیتابیس را کوئری می‌گیرد:

```csharp
// User is a custom class with fields for user data
internal User RetrieveUser (int id) { ... }
```

اگر این متد به دفعات فراخوانی شود، می‌توان عملکرد را با کش کردن نتایج در یک **Dictionary** ایستا بهبود داد. در اینجا یک راه‌حل ساده‌ی مفهومی آورده شده است که ایمنی نخ را نیز در نظر می‌گیرد:

```csharp
static class UserCache
{
  static Dictionary<int, User> _users = new Dictionary<int, User>();
  internal static User GetUser (int id)
  {
    User u = null;
    lock (_users)
      if (_users.TryGetValue (id, out u))
        return u;
    u = RetrieveUser (id);           // Method to retrieve from database;
    lock (_users) _users[id] = u;
    return u;
  }
}
```

در اینجا باید حداقل هنگام خواندن و به‌روزرسانی دیکشنری قفل‌گذاری کنیم تا ایمنی نخ تضمین شود. این طراحی یک مصالحه‌ی عملی میان سادگی و کارایی در قفل‌گذاری است. اما یک مشکل کوچک ایجاد می‌شود: اگر دو نخ به‌طور همزمان این متد را با یک شناسه‌ی یکسان (که قبلاً واکشی نشده) فراخوانی کنند، متد **RetrieveUser** دوبار اجرا می‌شود و دیکشنری بی‌دلیل به‌روزرسانی خواهد شد.

قفل کردن کل متد جلوی این مشکل را می‌گیرد، اما ناکارآمدی بیشتری ایجاد می‌کند: کل کش برای مدت فراخوانی **RetrieveUser** قفل می‌شود و در این مدت سایر نخ‌ها برای واکشی کاربران دیگر بلاک خواهند شد.

---

### راه‌حل ایده‌آل با Task<User> ⚡

برای یک راه‌حل ایده‌آل، باید استراتژی‌ای که در بخش «Completing synchronously» صفحه ۶۷۷ توضیح داده شد را به‌کار بگیریم. به جای کش کردن **User**، ما **Task<User>** را کش می‌کنیم و فراخواننده آن را **await** می‌کند:

```csharp
static class UserCache
{
  static Dictionary<int, Task<User>> _userTasks = 
     new Dictionary<int, Task<User>>();
  internal static Task<User> GetUserAsync (int id)
  {
    lock (_userTasks)
      if (_userTasks.TryGetValue (id, out var userTask))
        return userTask;
      else
        return _userTasks[id] = Task.Run(() => RetrieveUser(id));
  }
}
```

در این نسخه، یک قفل واحد کل منطق متد را پوشش می‌دهد. این کار به همروندی (concurrency) آسیبی نمی‌زند زیرا تنها کاری که داخل قفل انجام می‌دهیم، دسترسی به دیکشنری و (احتمالاً) شروع یک عملیات **asynchronous** با فراخوانی **Task.Run** است.

اگر دو نخ به‌طور همزمان این متد را با همان شناسه (ID) صدا بزنند، هر دو منتظر همان **Task** خواهند ماند؛ که دقیقاً همان چیزی است که می‌خواهیم. ✅

---

### اشیاء تغییرناپذیر (Immutable Objects) 🔒

یک **immutable object** شیئی است که وضعیتش (state) چه به‌صورت خارجی و چه داخلی، قابل تغییر نباشد. فیلدهای یک شیء immutable معمولاً **read-only** تعریف می‌شوند و در طول سازنده (constructor) مقداردهی کامل می‌شوند.

تغییرناپذیری (immutability) یکی از ویژگی‌های اصلی **برنامه‌نویسی تابعی (functional programming)** است—جایی که به‌جای تغییر دادن یک شیء، یک شیء جدید با ویژگی‌های متفاوت می‌سازید. **LINQ** از این پارادایم پیروی می‌کند.

تغییرناپذیری در برنامه‌های چندنخی نیز ارزشمند است زیرا مشکل **shared writable state** (اشتراک‌گذاری وضعیت قابل‌نوشتن) را از بین می‌برد یا به حداقل می‌رساند.

یک الگوی رایج این است که از اشیاء immutable برای کپسوله کردن گروهی از فیلدهای مرتبط استفاده کنید تا مدت زمان قفل‌گذاری کاهش یابد.

---

### یک مثال ساده 📊

فرض کنید دو فیلد زیر داریم:

```csharp
int _percentComplete;
string _statusMessage;
```

حالا اگر بخواهیم آن‌ها را به‌طور اتمی (atomic) بخوانیم و بنویسیم، به جای قفل‌گذاری مستقیم روی این فیلدها، می‌توانیم یک کلاس immutable تعریف کنیم:

```csharp
class ProgressStatus    // Represents progress of some activity
{
  public readonly int PercentComplete;
  public readonly string StatusMessage;
  // This class might have many more fields...
  public ProgressStatus (int percentComplete, string statusMessage)
  {
    PercentComplete = percentComplete;
    StatusMessage = statusMessage;
  }
}
```

سپس می‌توانیم یک فیلد از این نوع به همراه یک شیء قفل تعریف کنیم:

```csharp
readonly object _statusLocker = new object();
ProgressStatus _status;
```

اکنون می‌توانیم مقادیر این نوع را بدون نگه داشتن قفل برای مدت طولانی بخوانیم و بنویسیم:

```csharp
var status = new ProgressStatus (50, "Working on it");
// Imagine we were assigning many more fields...
// ...
lock (_statusLocker) _status = status;    // Very brief lock
```

برای خواندن شیء، ابتدا یک کپی از مرجع شیء را (داخل قفل) می‌گیریم. سپس می‌توانیم مقادیرش را بدون نیاز به نگه داشتن قفل بخوانیم:

```csharp
ProgressStatus status;
lock (_statusLocker) status = _status;   // Again, a brief lock
int pc = status.PercentComplete;
string msg = status.StatusMessage;
...
```

### قفل غیرانحصاری (Nonexclusive Locking) 🔓

ساختارهای قفل **غیرانحصاری** برای محدود کردن هم‌زمانی (concurrency) به‌کار می‌روند. در این بخش، به **Semaphore** و **Read/Writer Locks** می‌پردازیم و نشان می‌دهیم که چگونه کلاس **SemaphoreSlim** می‌تواند هم‌زمانی را در عملیات **آسنکرون** محدود کند.

---

### Semaphore 🕺

یک **Semaphore** شبیه یک **کلاب شبانه** با ظرفیت محدود است که توسط **دربان (bouncer)** مدیریت می‌شود.

* وقتی کلاب پر است، کسی نمی‌تواند وارد شود و صفی خارج از کلاب تشکیل می‌شود.
* تعداد Semaphore برابر است با تعداد جایگاه‌ها در کلاب.
* **Release** کردن یک Semaphore تعداد را افزایش می‌دهد؛ معمولاً وقتی کسی کلاب را ترک می‌کند (مطابقت با آزاد شدن یک منبع)، یا وقتی Semaphore مقداردهی اولیه می‌شود.
* می‌توان در هر زمان **Release** کرد تا ظرفیت افزایش یابد.

**Wait کردن روی یک Semaphore** تعداد آن را کاهش می‌دهد و معمولاً قبل از گرفتن یک منبع انجام می‌شود. اگر **Wait** روی Semaphore‌ای با مقدار فعلی بزرگ‌تر از صفر انجام شود، بلافاصله تکمیل می‌شود.

Semaphore می‌تواند حداکثر تعداد (maximum count) داشته باشد که یک محدودیت سخت محسوب می‌شود. افزایش مقدار بیش از این حد باعث پرتاب استثناء خواهد شد. هنگام ایجاد Semaphore، مقدار اولیه (initial count) و اختیاری حداکثر مقدار مشخص می‌شود.

---

یک **Semaphore** با مقدار اولیه یک شبیه **Mutex** یا **lock** است، با این تفاوت که **Semaphore** مالک (owner) ندارد و مستقل از نخ است. هر نخ می‌تواند **Release** کند، در حالی که در Mutex و lock فقط نخ دریافت‌کننده‌ی قفل می‌تواند آن را آزاد کند.

دو نسخه‌ی عملکردی مشابه وجود دارد: **Semaphore** و **SemaphoreSlim**. نسخه‌ی دوم برای برنامه‌نویسی موازی با تأخیر پایین بهینه شده است و در **multithreading** سنتی نیز مفید است، زیرا اجازه می‌دهد **CancellationToken** هنگام **Wait** مشخص شود و یک متد **WaitAsync** برای برنامه‌نویسی آسنکرون ارائه می‌دهد. اما برای سیگنال‌دهی بین فرآیندها (interprocess) کاربرد ندارد.

* **Semaphore** حدود ۱ میکروثانیه برای فراخوانی **WaitOne** و **Release** صرف می‌کند.
* **SemaphoreSlim** تقریباً یک‌دهم این زمان را مصرف می‌کند.

---

### کاربرد Semaphore برای محدود کردن هم‌زمانی ⚡

Semaphore برای جلوگیری از اجرای بیش از حد نخ‌ها روی یک بخش خاص از کد مفید است.

مثالی داریم که پنج نخ تلاش می‌کنند وارد یک کلاب شوند که فقط سه نخ همزمان اجازه ورود دارند:

```csharp
class TheClub
{
  static SemaphoreSlim _sem = new SemaphoreSlim(3);    // ظرفیت ۳
  static void Main()
  {
    for (int i = 1; i <= 5; i++) new Thread(Enter).Start(i);
  }

  static void Enter(object id)
  {
    Console.WriteLine(id + " wants to enter");
    _sem.Wait();
    Console.WriteLine(id + " is in!");           // حداکثر سه نخ
    Thread.Sleep(1000 * (int)id);               // می‌توانند همزمان
    Console.WriteLine(id + " is leaving");      // در کلاب باشند
    _sem.Release();
  }
}
```

نمونه خروجی ممکن:

```
1 wants to enter
1 is in!
2 wants to enter
2 is in!
3 wants to enter
3 is in!
4 wants to enter
5 wants to enter
1 is leaving
4 is in!
2 is leaving
5 is in!
```

---

همچنین قانونی است که Semaphore را با **مقدار اولیه صفر** ایجاد کنید و سپس با **Release** تعداد آن را افزایش دهید. مثال زیر دو Semaphore معادل را نشان می‌دهد:

```csharp
var semaphore1 = new SemaphoreSlim(3);
var semaphore2 = new SemaphoreSlim(0); 
semaphore2.Release(3);
```

اگر Semaphore نام‌گذاری شده باشد، می‌تواند مانند Mutex بین فرآیندها نیز مورد استفاده قرار گیرد. (Semaphore نام‌گذاری شده فقط در Windows موجود است، در حالی که Mutex نام‌گذاری شده روی Unix هم کار می‌کند.)
### Semaphoreها و قفل‌های آسنکرون (Asynchronous Semaphores and Locks) ⏳

قفل کردن (lock) **در یک عبارت await غیرمجاز است**:

```csharp
lock (_locker)
{
  await Task.Delay(1000);    // خطای کامپایل
  ...
}
```

دلیلش ساده است: قفل‌ها به یک **نخ خاص** تعلق دارند، و هنگام بازگشت از `await` معمولاً نخ تغییر می‌کند. علاوه بر این، **lock بلوک‌کننده است** و بلوک کردن برای یک بازه طولانی دقیقاً همان چیزی است که در برنامه‌های آسنکرون نمی‌خواهید.

---

با این حال، گاهی اوقات می‌خواهیم عملیات آسنکرون **به صورت متوالی اجرا شوند** یا تعداد عملیات همزمان را محدود کنیم تا بیش از n عملیات همزمان رخ ندهد.

مثال: یک مرورگر وب ممکن است نیاز داشته باشد تا دانلودها را به‌صورت آسنکرون و همزمان انجام دهد، اما بخواهد محدودیت حداکثر ۱۰ دانلود همزمان را اعمال کند. این کار را می‌توان با **SemaphoreSlim** انجام داد:

```csharp
SemaphoreSlim _semaphore = new SemaphoreSlim(10);

async Task<byte[]> DownloadWithSemaphoreAsync(string uri)
{
    await _semaphore.WaitAsync();
    try
    {
        return await new WebClient().DownloadDataTaskAsync(uri);
    }
    finally
    {
        _semaphore.Release();
    }
}
```

* اگر `initialCount` Semaphore را به ۱ کاهش دهیم، حداکثر هم‌زمانی به ۱ محدود می‌شود و عملاً یک **قفل آسنکرون** ایجاد می‌کند.

---

### نوشتن یک متد extension به نام `EnterAsync`

متد extension زیر استفاده آسنکرون از **SemaphoreSlim** را ساده‌تر می‌کند، با استفاده از کلاس **Disposable** که در بخش “Anonymous Disposal” معرفی شد:

```csharp
public static async Task<IDisposable> EnterAsync(this SemaphoreSlim ss)
{
    await ss.WaitAsync().ConfigureAwait(false);
    return Disposable.Create(() => ss.Release());
}
```

با این متد می‌توانیم روش قبلی دانلود را به شکل ساده‌تر بازنویسی کنیم:

```csharp
async Task<byte[]> DownloadWithSemaphoreAsync(string uri)
{
    using (await _semaphore.EnterAsync())
        return await new WebClient().DownloadDataTaskAsync(uri);
}
```

---

### Parallel.ForEachAsync

از **.NET 6**، روش دیگری برای محدود کردن هم‌زمانی آسنکرون وجود دارد: **Parallel.ForEachAsync**.

فرض کنید آرایه‌ای از URIها داریم که می‌خواهیم دانلود کنیم. می‌توانیم آنها را به‌صورت همزمان دانلود کنیم و هم‌زمانی را به حداکثر ۱۰ عملیات محدود کنیم:

```csharp
await Parallel.ForEachAsync(
    uris,
    new ParallelOptions { MaxDegreeOfParallelism = 10 },
    async (uri, cancelToken) =>
    {
        var download = await new HttpClient().GetByteArrayAsync(uri);
        Console.WriteLine($"Downloaded {download.Length} bytes");
    });
```

* سایر متدهای کلاس **Parallel** بیشتر برای سناریوهای برنامه‌نویسی موازی محاسباتی (**compute-bound**) استفاده می‌شوند، که در فصل ۲۲ بررسی شده‌اند.
### قفل‌های خواندن/نوشتن (Reader/Writer Locks) 📖

اغلب اوقات، نمونه‌های یک نوع داده **برای خواندن همزمان ایمن هستند**، اما **برای به‌روزرسانی همزمان یا ترکیبی از خواندن و نوشتن ایمن نیستند**.
این موضوع می‌تواند در مورد منابعی مانند فایل‌ها هم صادق باشد.

* استفاده از یک **قفل انحصاری ساده** (exclusive lock) معمولاً مشکل را حل می‌کند،
* اما اگر تعداد زیادی خواننده و فقط به‌روزرسانی‌های گاه‌به‌گاه داشته باشیم، این کار **محدودیت زیادی روی همزمانی ایجاد می‌کند**.

مثالی از چنین سناریویی در **سرورهای تجاری** است که داده‌های پرکاربرد را در **فیلدهای static** برای دسترسی سریع کش می‌کنند.

---

### کلاس `ReaderWriterLockSlim`

این کلاس برای **حداکثر دسترسی همزمان** در چنین شرایطی طراحی شده است و جایگزین کلاس قدیمی‌تر و “چاق” **ReaderWriterLock** است:

* کلاس قدیمی چندین بار کندتر بود و یک مشکل طراحی در ارتقای قفل داشت.
* در مقایسه با یک قفل معمولی (`Monitor.Enter/Exit`) هنوز دو برابر کندتر است، اما **مزیت آن کاهش رقابت (contention) هنگام خواندن زیاد و نوشتن کم است**.

---

### انواع قفل

دو نوع اصلی قفل داریم:

* **Write lock (قفل نوشتن)**: کاملاً انحصاری است.
* **Read lock (قفل خواندن)**: با سایر read lockها سازگار است.

> اگر یک نخ write lock داشته باشد، تمام نخ‌های دیگر که قصد read یا write دارند، مسدود می‌شوند. اما اگر هیچ write lockی وجود نداشته باشد، هر تعداد نخ می‌تواند همزمان read lock بگیرد.

---

### متدهای مهم `ReaderWriterLockSlim`

```csharp
public void EnterReadLock();
public void ExitReadLock();
public void EnterWriteLock();
public void ExitWriteLock();
```

* نسخه‌های **Try** هم وجود دارد که timeout می‌پذیرند (مشابه `Monitor.TryEnter`)
* کلاس قدیمی ReaderWriterLock روش‌های مشابهی به نام‌های `AcquireXXX` و `ReleaseXXX` دارد که در صورت timeout **ApplicationException** پرتاب می‌کند.

---

### مثال عملی

سه نخ مرتباً یک لیست را می‌خوانند و دو نخ دیگر هر ۱۰۰ میلی‌ثانیه یک عدد تصادفی به لیست اضافه می‌کنند:

```csharp
class SlimDemo
{
    static ReaderWriterLockSlim _rw = new ReaderWriterLockSlim();
    static List<int> _items = new List<int>();
    static Random _rand = new Random();

    static void Main()
    {
        new Thread(Read).Start();
        new Thread(Read).Start();
        new Thread(Read).Start();
        new Thread(Write).Start("A");
        new Thread(Write).Start("B");
    }

    static void Read()
    {
        while (true)
        {
            _rw.EnterReadLock();
            foreach (int i in _items) Thread.Sleep(10);
            _rw.ExitReadLock();
        }
    }

    static void Write(object threadID)
    {
        while (true)
        {
            int newNumber = GetRandNum(100);
            _rw.EnterWriteLock();
            _items.Add(newNumber);
            _rw.ExitWriteLock();
            Console.WriteLine("Thread " + threadID + " added " + newNumber);
            Thread.Sleep(100);
        }
    }

    static int GetRandNum(int max) { lock (_rand) return _rand.Next(max); }
}
```

* در کد تولیدی واقعی، معمولاً از **try/finally** برای اطمینان از آزاد شدن قفل‌ها در صورت بروز استثنا استفاده می‌کنیم.
* خروجی نمونه:

```
Thread B added 61
Thread A added 83
Thread B added 55
Thread A added 33
...
```

---

### مزیت اصلی

`ReaderWriterLockSlim` **امکان خواندن همزمان بیشتری نسبت به قفل ساده فراهم می‌کند**.

* برای مشاهده تعداد نخ‌های concurrent خواننده می‌توانیم در متد Write بنویسیم:

```csharp
Console.WriteLine(_rw.CurrentReadCount + " concurrent readers");
```

* اغلب اوقات این مقدار ۳ concurrent readers خواهد بود، زیرا متدهای Read بیشتر زمان خود را در حلقه `foreach` می‌گذرانند.

---

### ویژگی‌ها و پروپرتی‌های مانیتورینگ

```csharp
public bool IsReadLockHeld            { get; }
public bool IsUpgradeableReadLockHeld { get; }
public bool IsWriteLockHeld           { get; }
public int  WaitingReadCount          { get; }
public int  WaitingUpgradeCount       { get; }
public int  WaitingWriteCount         { get; }
public int  RecursiveReadCount        { get; }
public int  RecursiveUpgradeCount     { get; }
public int  RecursiveWriteCount       { get; }
```

این ویژگی‌ها به برنامه‌نویس امکان **مانیتور کردن وضعیت قفل‌ها** و بهینه‌سازی عملکرد را می‌دهد.
### قفل‌های قابل ارتقا (Upgradeable Locks) 🔄

گاهی لازم است که **یک read lock را به write lock تبدیل کنیم** به صورت **اتمی** (atomic).
مثلاً فرض کنید می‌خواهیم یک آیتم به لیست اضافه کنیم **فقط در صورتی که قبلاً وجود نداشته باشد**.

ایده این است که زمان نگه داشتن write lock را **به حداقل برسانیم**، و معمولاً مراحل زیر را دنبال می‌کنیم:

1. گرفتن یک **read lock**.
2. بررسی اینکه آیا آیتم قبلاً در لیست هست یا نه؛ اگر هست، قفل را آزاد کرده و باز می‌گردیم.
3. آزاد کردن read lock.
4. گرفتن **write lock**.
5. اضافه کردن آیتم به لیست.

💡 مشکل: بین مراحل ۳ و ۴، **یک نخ دیگر ممکن است وارد شده و لیست را تغییر دهد** (مثلاً همان آیتم را اضافه کند).

---

### راه‌حل `ReaderWriterLockSlim`

کلاس `ReaderWriterLockSlim` برای این مشکل، **نوع سومی از قفل** ارائه می‌دهد: **upgradeable lock**.

* این قفل شبیه read lock است، اما می‌توان آن را **بعداً به write lock ارتقا داد** به صورت اتمی.

مراحل استفاده از Upgradeable Lock:

1. فراخوانی `EnterUpgradeableReadLock()`
2. انجام عملیات مبتنی بر خواندن (مثلاً بررسی وجود آیتم)
3. فراخوانی `EnterWriteLock()` → این مرحله **قفل قابل ارتقا را به write lock تبدیل می‌کند**
4. انجام عملیات نوشتن (مثلاً اضافه کردن آیتم به لیست)
5. فراخوانی `ExitWriteLock()` → write lock به upgradeable lock باز می‌گردد
6. انجام هر عملیات خواندنی دیگر
7. فراخوانی `ExitUpgradeableReadLock()`

> از دید برنامه‌نویس، این مانند **قفل‌های تو در تو یا بازگشتی (nested/recursive)** است.
> از نظر عملکرد، در مرحله ۳، `ReaderWriterLockSlim` **read lock قبلی را آزاد کرده و write lock جدید می‌گیرد به صورت اتمی**.

---

### تفاوت مهم با read lock

* یک upgradeable lock می‌تواند **همزمان با تعداد زیادی read lock** وجود داشته باشد.
* اما **تنها یک upgradeable lock می‌تواند همزمان گرفته شود**.
* این محدودیت **از deadlock هنگام تبدیل جلوگیری می‌کند**، مشابه کاری که **update lock در SQL Server** انجام می‌دهد.
 
 <div align="center">
    
![Conventions-UsedThis-Book](../../assets/image/21/Table-21-1.jpeg) 
</div>

### مثال عملی از Upgradeable Lock و قفل بازگشتی 🔄

می‌توانیم **Upgradeable Lock** را با تغییر متد `Write` در مثال قبلی نشان دهیم، به طوری که **یک عدد به لیست اضافه شود فقط اگر قبلاً وجود نداشته باشد**:

```csharp
while (true)
{
    int newNumber = GetRandNum(100);
    _rw.EnterUpgradeableReadLock();

    if (!_items.Contains(newNumber))
    {
        _rw.EnterWriteLock();
        _items.Add(newNumber);
        _rw.ExitWriteLock();
        Console.WriteLine("Thread " + threadID + " added " + newNumber);
    }

    _rw.ExitUpgradeableReadLock();
    Thread.Sleep(100);
}
```

---

### قفل بازگشتی (Lock Recursion) 🔁

* `ReaderWriterLock` قدیمی می‌تواند تبدیل قفل‌ها را انجام دهد، اما **غیرقابل اعتماد** است و مفهوم **upgradeable lock** را ندارد.
* `ReaderWriterLockSlim` به طور پیش‌فرض **قفل‌های بازگشتی یا تو در تو را مجاز نمی‌داند**.

مثال خطا‌دهنده:

```csharp
var rw = new ReaderWriterLockSlim();
rw.EnterReadLock();
rw.EnterReadLock();  // Exception!
rw.ExitReadLock();
rw.ExitReadLock();
```

برای پشتیبانی از قفل بازگشتی، باید هنگام ساخت کلاس مشخص کنیم:

```csharp
var rw = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
```

💡 قانون اصلی برای قفل‌های بازگشتی: پس از گرفتن یک قفل، **قفل‌های بعدی می‌توانند کمتر اما نه بیشتر از نوع اولیه باشند**:

```
Read Lock → Upgradeable Lock → Write Lock
```

* ارتقاء upgradeable lock به write lock همیشه مجاز است.

مثال ترکیبی:

```csharp
rw.EnterWriteLock();
rw.EnterReadLock();
Console.WriteLine(rw.IsReadLockHeld);   // True
Console.WriteLine(rw.IsWriteLockHeld);  // True
rw.ExitReadLock();
rw.ExitWriteLock();
```

---

### سیگنال‌دهی با Event Wait Handles 🔔

**Event Wait Handle** ساده‌ترین ابزار برای سیگنال‌دهی بین نخ‌ها است و **مرتبط با C# events نیست**.
انواع اصلی:

* `AutoResetEvent`
* `ManualResetEvent` / `ManualResetEventSlim`
* `CountdownEvent`

تمام این‌ها از کلاس پایه `EventWaitHandle` مشتق شده‌اند.

---

#### AutoResetEvent 🎟️

* شبیه **گذرگاه بلیت (turnstile)** است: ورود بلیت فقط یک نفر را عبور می‌دهد.
* پس از عبور، گذرگاه به طور خودکار **reset** می‌شود.
* یک نخ با `WaitOne()` منتظر می‌ماند و یک نخ دیگر با `Set()` آن را آزاد می‌کند.

ساخت AutoResetEvent:

```csharp
var auto = new AutoResetEvent(false);  
// یا
var auto = new EventWaitHandle(false, EventResetMode.AutoReset);
```

مثال ساده:

```csharp
class BasicWaitHandle
{
    static EventWaitHandle _waitHandle = new AutoResetEvent(false);

    static void Main()
    {
        new Thread(Waiter).Start();
        Thread.Sleep(1000);       // مکث یک ثانیه
        _waitHandle.Set();        // فعال کردن Waiter
    }

    static void Waiter()
    {
        Console.WriteLine("Waiting...");
        _waitHandle.WaitOne();    // انتظار برای سیگنال
        Console.WriteLine("Notified");
    }
}
```

**خروجی:**

```
Waiting... (pause) Notified
```
 <div align="center">
    
![Conventions-UsedThis-Book](../../assets/image/21/Table-21-2.jpeg) 
</div>

### رفتار `Set` وقتی هیچ نخی منتظر نیست ⚠️

* اگر `Set()` فراخوانی شود و هیچ نخ منتظری وجود نداشته باشد، **handle باز می‌ماند** تا زمانی که یک نخ `WaitOne()` فراخوانی کند.
* این رفتار از **رقابت بین نخ‌ها جلوگیری می‌کند**: نخ می‌خواهد وارد گذرگاه شود و نخ دیگر زودتر `Set()` زده باشد.
* اما، **فراخوانی مکرر `Set()` وقتی هیچ‌کس منتظر نیست، باعث عبور چند نفر نمی‌شود**؛ فقط نفر بعدی عبور می‌کند و بلیت‌های اضافی «هدر می‌روند».

---

### آزادسازی Wait Handle ♻️

* پس از پایان استفاده از wait handle، می‌توان `Close()` را صدا زد تا منبع سیستم عامل آزاد شود.
* یا می‌توان تمام ارجاعات به آن را رها کرد و اجازه داد **garbage collector** بعداً آن را جمع‌آوری کند (wait handleها از الگوی disposal پیروی می‌کنند و finalizer آن‌ها `Close()` را فراخوانی می‌کند).
* Wait handleها هنگام خروج process به طور خودکار آزاد می‌شوند.

#### نکات مهم:

* `Reset()` روی AutoResetEvent، گذرگاه را می‌بندد (اگر باز باشد) بدون انتظار یا بلاک کردن.
* `WaitOne(timeout)` می‌تواند **مدت انتظار را محدود** کند و اگر timeout رخ دهد، `false` برمی‌گرداند.
* `WaitOne(0)` می‌تواند بررسی کند که آیا wait handle «باز» است بدون بلاک کردن نخ فراخواننده.

> ⚠️ توجه: این کار باعث **reset شدن AutoResetEvent** اگر باز باشد، می‌شود.

---

### سیگنال‌دهی دوطرفه ↔️

اگر بخواهیم نخ اصلی (main thread) به یک نخ worker سه بار متوالی سیگنال بدهد:

* فراخوانی ساده `Set()` چند بار پشت سر هم ممکن است باعث **از دست رفتن سیگنال‌ها** شود، زیرا worker ممکن است زمان ببرد تا هر سیگنال را پردازش کند.
* راه‌حل: نخ اصلی **صبر کند تا worker آماده شود** قبل از ارسال سیگنال. این کار با یک `AutoResetEvent` دیگر انجام می‌شود.

مثال کامل:

```csharp
class TwoWaySignaling
{
    static EventWaitHandle _ready = new AutoResetEvent(false);
    static EventWaitHandle _go = new AutoResetEvent(false);
    static readonly object _locker = new object();
    static string _message;

    static void Main()
    {
        new Thread(Work).Start();

        _ready.WaitOne();       // منتظر می‌ماند تا worker آماده شود
        lock (_locker) _message = "ooo";
        _go.Set();              // سیگنال به worker

        _ready.WaitOne();       // منتظر می‌ماند تا worker دوباره آماده شود
        lock (_locker) _message = "ahhh";
        _go.Set();

        _ready.WaitOne();
        lock (_locker) _message = null; // سیگنال پایان
        _go.Set();
    }

    static void Work()
    {
        while (true)
        {
            _ready.Set();       // اعلام آمادگی
            _go.WaitOne();      // منتظر سیگنال

            lock (_locker)
            {
                if (_message == null) return;  // خروج کنترل‌شده
                Console.WriteLine(_message);
            }
        }
    }
}
```

**خروجی:**

```
ooo
ahhh
```

* در شکل 21-2 فرآیند آماده شدن و سیگنال‌دهی دوطرفه نشان داده شده است.
 <div align="center">
    
![Conventions-UsedThis-Book](../../assets/image/21/Table-21-3.jpeg) 
</div>

### استفاده از پیام null برای پایان دادن به worker 🛑

* در مثال قبلی، از یک پیام `null` برای مشخص کردن پایان کار worker استفاده شد.
* برای نخ‌هایی که **به‌صورت نامحدود اجرا می‌شوند**، داشتن یک استراتژی خروج ضروری است.

---

### ManualResetEvent 🏗️

* همان‌طور که در فصل ۱۴ توضیح داده شد، `ManualResetEvent` مانند یک **دروازه ساده** عمل می‌کند:

  * فراخوانی `Set()` دروازه را باز می‌کند و **هر تعداد نخ** که `WaitOne()` فراخوانی کرده‌اند، عبور می‌دهند.
  * فراخوانی `Reset()` دروازه را می‌بندد. نخ‌هایی که `WaitOne()` روی دروازه بسته صدا می‌زنند، **بلاک می‌شوند** تا دروازه باز شود.
* عملکرد کلی مشابه `AutoResetEvent` است.

#### ساخت ManualResetEvent:

```csharp
var manual1 = new ManualResetEvent(false);
var manual2 = new EventWaitHandle(false, EventResetMode.ManualReset);
```

* نسخه بهینه‌تری به نام `ManualResetEventSlim` وجود دارد که برای **زمان انتظار کوتاه** بهینه شده و امکان **استفاده از CancellationToken** را دارد.
* `ManualResetEventSlim` subclass از `WaitHandle` نیست اما دارای ویژگی `WaitHandle` است که یک **object مبتنی بر WaitHandle** برمی‌گرداند.

---

### عملکرد و کارایی ⏱️

* انتظار یا سیگنال دادن با `AutoResetEvent` یا `ManualResetEvent` حدود **یک میکروثانیه** طول می‌کشد (اگر بلاک نشود).
* `ManualResetEventSlim` و `CountdownEvent` می‌توانند **تا ۵۰ برابر سریع‌تر** در زمان انتظار کوتاه عمل کنند، به دلیل عدم وابستگی به OS و استفاده از **spinning constructs**.
* با این حال، در اکثر سناریوها، overhead این کلاس‌ها معمولاً **گلوگاه ایجاد نمی‌کند**.

---

### کاربرد ManualResetEvent و CountdownEvent

* `ManualResetEvent`: برای **باز کردن یک نخ برای چند نخ دیگر** مفید است.
* `CountdownEvent`: برای **منتظر ماندن روی چند نخ** استفاده می‌شود.

#### CountdownEvent

```csharp
var countdown = new CountdownEvent(3); // مقدار اولیه ۳ نخ

new Thread(SaySomething).Start("I am thread 1");
new Thread(SaySomething).Start("I am thread 2");
new Thread(SaySomething).Start("I am thread 3");

countdown.Wait(); // بلوک تا ۳ بار Signal فراخوانی شود
Console.WriteLine("All threads have finished speaking!");

void SaySomething(object thing)
{
    Thread.Sleep(1000);
    Console.WriteLine(thing);
    countdown.Signal(); // کاهش count
}
```

* می‌توان count را با `AddCount` افزایش داد، اما اگر شمارش به صفر رسیده باشد، **استثنا ایجاد می‌کند**.
* برای جلوگیری از خطا، از `TryAddCount` استفاده می‌کنیم که **false** برمی‌گرداند اگر شمارش صفر باشد.
* برای “unsignal” کردن یک CountdownEvent از `Reset()` استفاده می‌کنیم که **count را به مقدار اولیه بازنشانی می‌کند**.
* مشابه `ManualResetEventSlim`، CountdownEvent دارای ویژگی `WaitHandle` برای تعامل با کلاس‌هایی که **WaitHandle انتظار دارند**، است.

---

### ایجاد EventWaitHandle میان پردازشی 🌐

* می‌توان یک EventWaitHandle **نام‌دار** ایجاد کرد که بین چند پردازش کار کند.
* نام تنها یک رشته است و باید **منحصربه‌فرد باشد** تا با سایر منابع تداخل نکند.

```csharp
EventWaitHandle wh = new EventWaitHandle(
    false,
    EventResetMode.AutoReset,
    @"Global\MyCompany.MyApp.SomeName"
);
```

* اگر دو برنامه این کد را اجرا کنند، می‌توانند **به هم سیگنال دهند**: wait handle در همه نخ‌ها و پردازش‌ها کار می‌کند.
* توجه: **Named EventWaitHandle فقط در Windows موجود است**.
### Wait Handles و Continuations 🔄

به جای **منتظر ماندن روی یک wait handle** و بلاک کردن نخ، می‌توان یک **continuation** به آن ضمیمه کرد با استفاده از `ThreadPool.RegisterWaitForSingleObject`.

#### مثال:

```csharp
var starter = new ManualResetEvent(false);

RegisteredWaitHandle reg = ThreadPool.RegisterWaitForSingleObject(
    starter, Go, "Some Data", -1, true);

Thread.Sleep(5000);
Console.WriteLine("Signaling worker...");
starter.Set();
Console.ReadLine();

reg.Unregister(starter); // پاک‌سازی پس از اتمام

void Go(object data, bool timedOut)
{
    Console.WriteLine("Started - " + data);
    // انجام کار...
}
```

* زمانی که wait handle **signaled** می‌شود (یا timeout رخ می‌دهد)، delegate روی یک نخ از ThreadPool اجرا می‌شود.
* پس از آن باید `Unregister` فراخوانی شود تا **handle غیرمدیریت‌شده** آزاد شود.
* این متد همچنین یک **object “black box”** می‌گیرد که به delegate منتقل می‌شود، یک **timeout** (میلی‌ثانیه، `-1` یعنی بدون timeout) و یک **Boolean** که مشخص می‌کند آیا فراخوانی یک‌باره است یا مکرر.

⚠️ نکته:

* می‌توان `RegisterWaitForSingleObject` را **فقط یک بار برای هر wait handle** فراخوانی کرد.
* فراخوانی دوباره روی همان wait handle ممکن است باعث شود callback حتی وقتی handle signaled نشده، اجرا شود.
* به همین دلیل، wait handles غیر-Slim برای برنامه‌نویسی asynchronous مناسب نیستند.

---

### WaitAny، WaitAll و SignalAndWait ⏳🔄

علاوه بر متدهای **Set**، **WaitOne** و **Reset**، کلاس **WaitHandle** دارای متدهای استاتیک دیگری است که برای حل مسائل پیچیده‌تر همزمان‌سازی کاربرد دارند. متدهای **WaitAny**، **WaitAll** و **SignalAndWait** عملیات‌های سیگنال‌دهی و انتظار را روی چندین **wait handle** انجام می‌دهند. این **wait handle**ها می‌توانند از انواع مختلف باشند (از جمله **Mutex** و **Semaphore**، زیرا این‌ها نیز از کلاس انتزاعی **WaitHandle** مشتق شده‌اند).

همچنین، **ManualResetEventSlim** و **CountdownEvent** می‌توانند از طریق ویژگی‌های **WaitHandle** خود در این متدها شرکت کنند.

متدهای **WaitAll** و **SignalAndWait** ارتباط عجیبی با معماری قدیمی **COM** دارند: این متدها نیازمند آن هستند که فراخواننده در یک **multithreaded apartment** باشد، مدلی که برای تعامل با دیگر سیستم‌ها چندان مناسب نیست. به‌عنوان مثال، **thread اصلی** یک برنامه **WPF** یا **Windows Forms** در این حالت نمی‌تواند با **clipboard** تعامل کند. جایگزین‌ها را در ادامه بررسی خواهیم کرد.

* **WaitHandle.WaitAny** منتظر می‌ماند تا هر یک از آرایه‌ای از **wait handle**ها سیگنال بگیرند.
* **WaitHandle.WaitAll** منتظر می‌ماند تا همه **wait handle**های داده‌شده به‌صورت **اتمی** سیگنال بگیرند.

این بدان معناست که اگر روی دو **AutoResetEvent** منتظر باشید:

* **WaitAny** هرگز باعث نمی‌شود که هر دو رویداد به‌طور همزمان "قفل" شوند.
* **WaitAll** هرگز باعث نمی‌شود که تنها یکی از رویدادها "قفل" شود.

**SignalAndWait** ابتدا **Set** را روی یک **WaitHandle** فراخوانی می‌کند و سپس **WaitOne** را روی **WaitHandle** دیگر صدا می‌زند. پس از سیگنال‌دهی به **handle** اول، به ابتدای صف انتظار برای **handle** دوم می‌رود؛ این کار کمک می‌کند تا عملیات موفق شود (هرچند که عملیات واقعاً اتمی نیست). می‌توان این متد را به‌عنوان «تبادل یک سیگنال با سیگنال دیگر» تصور کرد و آن را روی یک جفت **EventWaitHandle** استفاده کرد تا دو **thread** در یک نقطه زمانی با هم ملاقات یا **rendezvous** داشته باشند. هم **AutoResetEvent** و هم **ManualResetEvent** مناسب این کار هستند.

* **Thread اول** اجرا می‌کند:

```csharp
WaitHandle.SignalAndWait (wh1, wh2);
```

* **Thread دوم** کار معکوس را انجام می‌دهد:

```csharp
WaitHandle.SignalAndWait (wh2, wh1);
```

---

### جایگزین‌ها برای WaitAll و SignalAndWait 🔁

متدهای **WaitAll** و **SignalAndWait** در یک **single-threaded apartment** کار نمی‌کنند. خوشبختانه جایگزین‌هایی وجود دارند. در مورد **SignalAndWait**، به ندرت نیاز به ویژگی‌های **queue-jumping** آن است: برای مثال در مثال **rendezvous** ما، کافی است که ابتدا **Set** را روی **wait handle** اول صدا بزنید و سپس **WaitOne** را روی دیگری فراخوانی کنید، اگر از **wait handle**ها صرفاً برای همان ملاقات استفاده می‌کنید. در بخش بعد، یک گزینه دیگر برای پیاده‌سازی **thread rendezvous** بررسی می‌کنیم.

در مورد **WaitAny** و **WaitAll**، اگر به اتمی بودن نیاز ندارید، می‌توانید از کدی که در بخش قبل نوشتیم استفاده کنید تا **wait handle**ها را به **Task** تبدیل کرده و سپس از **Task.WhenAny** و **Task.WhenAll** استفاده کنید (فصل ۱۴).

اگر به اتمی بودن نیاز دارید، می‌توانید از سطح پایین‌تر شروع کنید و منطق سیگنال‌دهی را خودتان با متدهای **Monitor.Wait** و **Monitor.Pulse** بنویسید. ما **Wait** و **Pulse** را به‌صورت مفصل در [این لینک](http://albahari.com/threading) توضیح داده‌ایم. ✅

### کلاس Barrier 🛑🧵

کلاس **Barrier** یک **مانع اجرای thread** پیاده‌سازی می‌کند که اجازه می‌دهد چندین **thread** در یک نقطه زمانی با هم **rendezvous** داشته باشند (این با **Thread.MemoryBarrier** متفاوت است).

این کلاس بسیار سریع و کارآمد است و بر اساس متدهای **Wait**، **Pulse** و **spinlock** ساخته شده است.

#### نحوه استفاده:

1. یک نمونه از کلاس ایجاد کنید و مشخص کنید که چند **thread** باید در **rendezvous** شرکت کنند (می‌توانید بعداً با متدهای **AddParticipants** یا **RemoveParticipants** این تعداد را تغییر دهید).
2. هر **thread** زمانی که می‌خواهد در **rendezvous** شرکت کند، متد **SignalAndWait** را فراخوانی کند.

اگر کلاس **Barrier** را با مقدار ۳ ایجاد کنید، فراخوانی **SignalAndWait** تا زمانی که این متد سه بار فراخوانی نشده باشد، **بلوک** می‌شود. سپس دوباره چرخه شروع می‌شود: فراخوانی بعدی **SignalAndWait** دوباره تا سه بار صدا زدن متد **بلوک** می‌شود. این کار باعث می‌شود هر **thread** همزمان با دیگر **thread**ها حرکت کند.

#### مثال عملی:

در این مثال، هر یک از سه **thread** اعداد ۰ تا ۴ را چاپ می‌کنند و هم‌زمان با دیگر **thread**ها جلو می‌روند:

```csharp
var barrier = new Barrier(3);
new Thread(Speak).Start();
new Thread(Speak).Start();
new Thread(Speak).Start();

void Speak()
{
    for (int i = 0; i < 5; i++)
    {
        Console.Write(i + " ");
        barrier.SignalAndWait();
    }
}
```

**خروجی:**

```
0 0 0 1 1 1 2 2 2 3 3 3 4 4 4
```

یک ویژگی بسیار مفید **Barrier** این است که می‌توانید **post-phase action** را هنگام ایجاد آن مشخص کنید. این یک **delegate** است که بعد از آنکه **SignalAndWait** به تعداد مشخص فراخوانی شد اجرا می‌شود، اما قبل از اینکه **threads** آزاد شوند.

در مثال ما، اگر **Barrier** را به این صورت ایجاد کنیم:

```csharp
static Barrier _barrier = new Barrier(3, barrier => Console.WriteLine());
```

خروجی به صورت خط به خط خواهد بود:

```
0 0 0 
1 1 1 
2 2 2 
3 3 3 
4 4 4
```

این ویژگی باعث می‌شود هماهنگی **threads** بسیار منظم و خوانا باشد. ✅

 <div align="center">
    
![Conventions-UsedThis-Book](../../assets/image/21/Table-21-4.jpeg) 
</div>

