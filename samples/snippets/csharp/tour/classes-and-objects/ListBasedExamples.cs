﻿namespace ListExamples
{
    using System;
    public class MyList<T>
    {
        // Constant
        const int defaultCapacity = 4;

        // Fields
        T[] items;
        int count;

        // Constructor
        public MyList(int capacity = defaultCapacity) 
        {
            items = new T[capacity];
        }

        // Properties
        public int Count => count; 

        public int Capacity 
        {
            get { return items.Length; }
            set 
            {
                if (value < count) value = count;
                if (value != items.Length) 
                {
                    T[] newItems = new T[value];
                    Array.Copy(items, 0, newItems, 0, count);
                    items = newItems;
                }
            }
        }

        // Indexer
        public T this[int index] 
        {
            get 
            {
                return items[index];
            }
            set 
            {
                items[index] = value;
                OnChanged();
            }
        }
        
        // Methods
        public void Add(T item) 
        {
            if (count == Capacity) Capacity = count * 2;
            items[count] = item;
            count++;
            OnChanged();
        }
        protected virtual void OnChanged() =>
            Changed?.Invoke(this, EventArgs.Empty);

        public override bool Equals(object other) =>
            Equals(this, other as MyList<T>);

        static bool Equals(MyList<T> a, MyList<T> b) 
        {
            if (Object.ReferenceEquals(a, null)) return Object.ReferenceEquals(b, null);
            if (Object.ReferenceEquals(b, null) || a.count != b.count)
                return false;
            for (int i = 0; i < a.count; i++) 
            {
                if (!object.Equals(a.items[i], b.items[i])) 
                {
                    return false;
                }
            }
        return true;
        }

        // Event
        public event EventHandler Changed;

        // Operators
        public static bool operator ==(MyList<T> a, MyList<T> b) => 
            Equals(a, b);

        public static bool operator !=(MyList<T> a, MyList<T> b) => 
            !Equals(a, b);
    }

    public class ExampleCode
    {
        public static void ListExampleOne()
        {
            MyList<string> list1 = new MyList<string>();
            MyList<string> list2 = new MyList<string>(10);
        }

        public static void ListExampleTwo()
        {
            MyList<string> names = new MyList<string>();
            names.Capacity = 100;   // Invokes set accessor
            int i = names.Count;    // Invokes get accessor
            int j = names.Capacity; // Invokes get accessor
        }

        public static void ListExampleThree()
        {
            MyList<string> names = new MyList<string>();
            names.Add("Liz");
            names.Add("Martha");
            names.Add("Beth");
            for (int i = 0; i < names.Count; i++) 
            {
                string s = names[i];
                names[i] = s.ToUpper();
            }
        }
        public static void ListExampleFour() 
        {
            MyList<int> a = new MyList<int>();
            a.Add(1);
            a.Add(2);
            MyList<int> b = new MyList<int>();
            b.Add(1);
            b.Add(2);
            Console.WriteLine(a == b);  // Outputs "True" 
            b.Add(3);
            Console.WriteLine(a == b);  // Outputs "False"
        }
    }
    class EventExample
    {
        static int changeCount;
        static void ListChanged(object sender, EventArgs e) 
        {
            changeCount++;
        }
        public static void Usage() 
        {
            MyList<string> names = new MyList<string>();
            names.Changed += new EventHandler(ListChanged);
            names.Add("Liz");
            names.Add("Martha");
            names.Add("Beth");
            Console.WriteLine(changeCount);		// Outputs "3"
        }
    }
}