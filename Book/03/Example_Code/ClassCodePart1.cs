/*
این فایل برای درک بهتر متن های کتاب ساخته شده است این کد ها توسط مترجم به صورت جدا نوشته شده است 
و ارتباط مستقیمی با نویسنده و کتاب اصلی ندارد
با تشکر مترجم🌹
*/
/*
 در این نمونه ها تلاش کردم که بخش های که نیاز به نمونه بیشتر دارند را در اینجا بیاورم 
 در صورت هر گونه ایده و مشکلی سپاس گذارم یک ایشو باز کنید
*/
//این کد فقط برای نمونه است

// اضافه کردن کتابخانه های استاندارد دات نت : 
using System;
using System.Collections.Generic;
using System.Linq;

    // 1. سازنده‌های اصلی (Primary Constructors)
    // یک کلاس ساده با سازنده اصلی
    public class Person(string firstName, string lastName)
    {
        // دسترسی به پارامترهای سازنده اصلی در متدها
        public void PrintFullName()
        {
            Console.WriteLine($"Full Name: {firstName} {lastName}");
        }

        // استفاده از پارامترهای سازنده اصلی برای مقداردهی فیلدها
        public readonly string FirstName = firstName;
        public string LastNameProperty { get; } = lastName;

        // یک سازنده دیگر که سازنده اصلی را فراخوانی می‌کند
        public Person(string firstName, string lastName, int age) : this(firstName, lastName)
        {
            Console.WriteLine($"Person {firstName} {lastName} with age {age} created.");
        }
    }

    // 2. پادسازنده‌ها (Deconstructors)
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        // متد پادسازنده
        public void Deconstruct(out int x, out int y)
        {
            x = X;
            y = Y;
        }

        // پادسازنده با پارامترهای بیشتر (پربارسازی)
        public void Deconstruct(out int x, out int y, out string description)
        {
            x = X;
            y = Y;
            description = $"Point ({X}, {Y})";
        }
    }

    // کلاس کمکی برای متد گسترشی پادسازنده
    public static class PointExtensions
    {
        public static void Deconstruct(this Point p, out double distance)
        {
            distance = Math.Sqrt(p.X * p.X + p.Y * p.Y);
        }
    }

    // 3. ایندکسرها (Indexers)
    public class StringCollection
    {
        private List<string> _data = new List<string>();

        public StringCollection(params string[] initialData)
        {
            _data.AddRange(initialData);
        }

        // ایندکسر پایه با یک پارامتر
        public string this[int index]
        {
            get
            {
                if (index < 0 || index >= _data.Count)
                    throw new IndexOutOfRangeException("Index out of range.");
                return _data[index];
            }
            set
            {
                if (index < 0 || index >= _data.Count)
                    throw new IndexOutOfRangeException("Index out of range.");
                _data[index] = value;
            }
        }

        // ایندکسر با چندین پارامتر (مثلاً برای یک جدول ساده)
        public string this[int row, int col]
        {
            get
            {
                // فرض می‌کنیم داده‌ها به صورت خطی ذخیره شده‌اند و هر سطر 3 ستون دارد
                int actualIndex = row * 3 + col;
                if (actualIndex < 0 || actualIndex >= _data.Count)
                    throw new IndexOutOfRangeException("Index out of range.");
                return _data[actualIndex];
            }
            set
            {
                int actualIndex = row * 3 + col;
                if (actualIndex < 0 || actualIndex >= _data.Count)
                    throw new IndexOutOfRangeException("Index out of range.");
                _data[actualIndex] = value;
            }
        }

        // ایندکسر با Index (از C# 8)
        public string this[Index index] => _data[index];

        // ایندکسر با Range (از C# 8)
        public List<string> this[Range range] => _data.ToArray()[range].ToList();
    }

    // 4. تنظیم‌کننده‌های تنها-آغازگر (Init-only Setters)
    public class Product
    {
        // ویژگی تنها-آغازگر ساده
        public int ProductId { get; init; }

        // ویژگی تنها-آغازگر با مقدار پیش‌فرض
        public string Name { get; init; } = "Unknown";

        // ویژگی تنها-آغازگر با فیلد پشتیبان
        private decimal _price;
        public decimal Price
        {
            get => _price;
            init => _price = value > 0 ? value : throw new ArgumentOutOfRangeException(nameof(value), "Price must be positive.");
        }

        // یک ویژگی خواندنی-تنها (برای مقایسه)
        public string Category { get; }

        public Product(string category)
        {
            Category = category;
        }
    }

    // کلاس اصلی برنامه که نقطه شروع اجرا است
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("--- Primary Constructors Examples ---");
            Person p1 = new Person("Khashayar", "AP");
            p1.PrintFullName();
            Console.WriteLine($"First Name (field): {p1.FirstName}");
            Console.WriteLine($"Last Name (property): {p1.LastNameProperty}");

            Person p2 = new Person("Hamed", "Csharp", 20);

            Console.WriteLine("\n--- Deconstructors Examples ---");
            Point point = new Point(10, 20);

            // پادسازی به متغیرهای جدید
            var (x, y) = point;
            Console.WriteLine($"Point (x, y): ({x}, {y})");

            // پادسازی با نادیده گرفتن متغیر
            var (_, yOnly) = point;
            Console.WriteLine($"Only y coordinate: {yOnly}");

            // پادسازی به متغیرهای موجود
            int existingX = 0, existingY = 0;
            (existingX, existingY) = point;
            Console.WriteLine($"Existing point (x, y): ({existingX}, {existingY})");

            // پادسازی با پربارسازی
            var (xFull, yFull, desc) = point;
            Console.WriteLine($"Full point: {desc}");

            // پادسازی با متد گسترشی
            double distance; // تعریف متغیر
            point.Deconstruct(out distance); // فراخوانی آشکار متد گسترشی
            Console.WriteLine($"Distance from origin: {distance:F2}");


            Console.WriteLine("\n--- Indexers Examples ---");
            StringCollection collection = new StringCollection("C", "Sharp", "Language", "Programming", "Powerful");

            Console.WriteLine($"Element at index 1: {collection[1]}");
            collection[1] = "Python";
            Console.WriteLine($"Element at index 1 after change: {collection[1]}");

            // استفاده از ایندکسر چند پارامتری (فرضی)
            StringCollection table = new StringCollection("Row0-Col0", "Row0-Col1", "Row0-Col2",
                                                          "Row1-Col0", "Row1-Col1", "Row1-Col2");
            Console.WriteLine($"Element at row 1, column 0: {table[1, 0]}");

            // استفاده از Index (از C# 8)
            Console.WriteLine($"Last element: {collection[^1]}");

            // استفاده از Range (از C# 8)
            List<string> firstTwo = collection[..2];
            Console.WriteLine($"First two elements: {string.Join(", ", firstTwo)}");

            List<string> lastThree = collection[^3..];
            Console.WriteLine($"Last three elements: {string.Join(", ", lastThree)}");


            Console.WriteLine("\n--- Init-only Setters Examples ---");
            // مقداردهی با آغازگر شیء
            Product product1 = new Product("Electronics")
            {
                ProductId = 101,
                Name = "Laptop",
                Price = 1500.00m
            };
            Console.WriteLine($"Product 1: ID {product1.ProductId}, Name {product1.Name}, Price {product1.Price:C}, Category {product1.Category}");           

            // مقداردهی با مقدار پیش‌فرض
            Product product2 = new Product("Book");
            Console.WriteLine($"Product 2: ID {product2.ProductId}, Name {product2.Name}, Price {product2.Price:C}, Category {product2.Category}");

            // ویژگی با اعتبارسنجی در init
            try
            {
                Product product3 = new Product("Food") { Price = -10.00m };
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Error creating product: {ex.Message}");
            }
        }
    }
