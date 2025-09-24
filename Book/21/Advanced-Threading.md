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

