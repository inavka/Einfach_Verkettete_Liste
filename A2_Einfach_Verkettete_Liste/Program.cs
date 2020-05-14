using System;

namespace A2_Einfach_Verkettete_Liste
{
    class Program
    {
        class WordCount
        {
            class LItem
            {
                public string text;
                public int count;
                public LItem next;
                public LItem(string text, int count = 1)
                {
                    this.text = text;
                    this.count = count;
                }
                public override string ToString() => $"{text} -- {count}";
            }

            LItem first = null, last = null;
            public WordCount() { }
        
            public WordCount(params string[] textList)
            {
                if (textList.Length < 1)
                    throw new ArgumentNullException("Die Wortliste ist leer");
                SortedUpdate(textList);
            }

            private void AddLast(string text, int count = 1)
            {
                LItem newItem = new LItem(text, count);
                if (first == null)                  //1. Fall: die Liste ist leer
                    first = last = newItem;
                else
                {
                    last.next = newItem;            //2. Fall: die Liste hat min. 1 Element
                    last = last.next;
                }
            }

            private void AddFirst(string text, int count = 1)
            {
                LItem newItem = new LItem(text, count);
                if (first == null)                  //1. Fall: die Liste ist leer
                    first = last = newItem;
                else
                {
                    newItem.next = first;           //2. Fall: die Liste hat min. 1 Element
                    first = newItem;
                }
            }

            public void SortedUpdate(params string[] text)
            {
                foreach (string t in text)
                {
                    SortedUpdate(t);
                }
            }

            public void SortedUpdate(string text)
            {
                bool found = false;                             //Prueft ob das Wort schon in der Liste enthalten ist
                for (LItem item = first; item != null; item = item.next)
                {
                    if (text.CompareTo(item.text) == 0)
                    {
                        item.count++;                           
                        found = true;
                    }
                }

                if (!found)                                     //Hier: Die Liste enthaelt das Wort nicht, es muss
                {                                               //entsprechend hingefuegt werden
                    // 1.Fall: Leere Liste oder neues Item nach dem letzten Item
                    if (first == null || text.CompareTo(last.text) > 0)
                        AddLast(text);

                    // 2.Fall: Neues Item vor dem ersten Item
                    else if (text.CompareTo(first.text) < 0)
                        AddFirst(text);

                    // 3.Fall: Neues Item soll in die "Mitte"
                    else
                    {
                        LItem newItem = new LItem(text);
                        LItem tmp = first;
                        while (tmp.next.text.CompareTo(text) < 0) // tmp verweist auf das Element VOR der Einfügestelle
                            tmp = tmp.next;

                        newItem.next = tmp.next;
                        tmp.next = newItem;
                    }
                }
            }

            public void Print(int min = 1)
            {
                for (LItem tmp = first; tmp != null; tmp = tmp.next)
                {
                    if (tmp.count >= min)
                        Console.WriteLine(tmp);
                }
            }
            public void Reverse()
            {
                if (first == last) //1.Fall: die Liste ist Leer
                    return;
                
                //2. Fall: Liste hat mehr als 1 Element
                LItem current = first;
                LItem newFirst = null;
                while (current != null)
                {
                    LItem tmpNext = current.next;
                    current.next = newFirst;
                    newFirst = current;
                    current = tmpNext;
                }
                last = first; //Neues Ende ist der alte Anfang
                first = newFirst;
            }

            public WordCount Filter(string pattern)
            {
                WordCount list = new WordCount();
                for (LItem item = first; item != null; item = item.next)
                {
                    if (item.text.Contains(pattern))
                    {
                        list.SortedUpdate(item.text);
                    }  
                }
                return list;
            }

            public void DeleteFirst()
            {
                if (first == null)  // 1.Fall: Liste ist leer
                    return;
                if (first == last)  // 2. Fall: Liste besteht nur aus einem Element
                    first = last = null;
                else
                    first = first.next;   // 3. Fall: Liste hat mehr als ein Element
            }

            public void DeleteLast()
            {
                if (first == null)  // 1. Fall: Liste ist leer
                    throw new NullReferenceException("Die Liste ist leer");
                if (first == last)  // 2. Fall: Liste besteht nur aus einem Element
                    first = last = null;
                else              // 3. Fall: Liste hat mehr als ein Element
                {  
                    LItem nextToLast = first; //das vorletzte Element
                    while (nextToLast.next != last)
                        nextToLast = nextToLast.next;
                    nextToLast.next = null;
                    last = nextToLast;
                }
            }
            public void DeleteNth(string text)
            {
                LItem tmp;
                bool found = false;
                for (tmp = first; tmp != null; tmp = tmp.next)
                {
                    if (tmp.text.CompareTo(text) == 0) //Prueft ob das Wort in der Liste enthalten ist
                    {
                        found = true;
                        break;
                    }
                }
                if (found)
                {
                    LItem nextToNth = first;
                    while (nextToNth.next != tmp)
                        nextToNth = nextToNth.next;
                    nextToNth.next = nextToNth.next.next;
                }
                else
                    return;
            }

            public void Delete(string text)
            {
                if (first == null)            
                    return;
                else if (text.CompareTo(first.text) == 0)
                    DeleteFirst();
                else if (text.CompareTo(last.text) == 0)
                    DeleteLast();
                else
                    DeleteNth(text);
            }

            private LItem NthElem(string text)
            {
                LItem tmp = first;
                while (text.CompareTo(first.text) != 0)
                {
                    tmp = tmp.next;
                }
                return tmp;
            }
            public string this[string text]
            {
                get => $"{NthElem(text).count}";
                
            }
        }
        static void Main(string[] args)
        {
            string[] text =
        {
            "Zu Dionys, dem Tyrannen, schlich ",
            "Damon, den Dolch im Gewande: ",
            "Ihn schlugen die Häscher in Bande, ",
            "'Was wolltest du mit dem Dolche? sprich!'",
            "Entgegnet ihm finster der Wüterich.",
            "'Die Stadt vom Tyrannen befreien!'",
            "'Das sollst du am Kreuze bereuen.'",

            "Ich bin, spricht jener, zu sterben bereit ",
            "Und bitte nicht um mein Leben: ",
            "Doch willst du Gnade mir geben,",
            "Ich flehe dich um drei Tage Zeit,",
            "Bis ich die Schwester dem Gatten gefreit; ",
            "Ich lasse den Freund dir als Bürgen, ",
            "Ihn magst du, entrinn' ich, erwürgen.'",

            "Da lächelt der König mit arger List",
            "Und spricht nach kurzem Bedenken: ",
            "'Drei Tage will ich dir schenken; ",
            "Doch wisse, wenn sie verstrichen, die Frist, ",
            "Eh' du zurück mir gegeben bist, ",
            "So muß er statt deiner erblassen,",
            "Doch dir ist die Strafe erlassen.'"
        };

            WordCount count1 = new WordCount("Alf", "Bart", "Charlie", "Dora", "Emil", "Bart", "Charlie", "Dora", "Emil", "Charlie", "Dora", "Emil", "Dora", "Emil", "Emil");
            WordCount count2 = new WordCount();
            char[] seperatoren = { '\'', ',', '.', '?', '!', ' ', ';' };
            foreach (var zeile in text)
            {
                count2.SortedUpdate(zeile.Split(seperatoren, StringSplitOptions.RemoveEmptyEntries));
            }
            count1.Print();
            Console.WriteLine("---------");
            count1.Reverse();
            count1.Delete("Alf");
            count1.Delete("Emil");
            count1.Delete("Charlie");
            count1.Print();
            Console.WriteLine(count1["Dora"]);
            Console.WriteLine("---------");
            count2.Print(3);
            Console.WriteLine("---------");
            WordCount count3 = count2.Filter("ich");
            count3.Print();

            Console.ReadKey();
        }
    }
}
