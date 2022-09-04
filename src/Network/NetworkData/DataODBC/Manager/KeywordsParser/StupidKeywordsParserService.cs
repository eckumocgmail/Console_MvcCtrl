using System.Collections.Generic;

namespace KeywordsPW
{
   using System;
   using System.Collections.Generic;

   public class StupidKeywordsParserService : IKeywordsParserService
   {

      private IDictionary<string,int> keywords =  new Dictionary<string, int>();

      public IDictionary<string, int> ParseKeywords(string Resource)
      {
         
         var statisticsForThisRecord = new Dictionary<string, int>();
         
         foreach (string text in Resource.SplitWords())
         {
            string word = text.ToUpper();
            if (keywords.ContainsKey(word))
            {
                keywords[word]++;
            }
            else
            {
                keywords[word] = 1;
            }

            if (statisticsForThisRecord.ContainsKey(word))
            {
                statisticsForThisRecord[word]++;
            }
            else
            {
                statisticsForThisRecord[word] = 1;
            }
         }
         return statisticsForThisRecord;
      }
   }

}
public static class TextExtension
{

    public static bool IsEngChar(char ch) => ("qwertyuiopasdfghjklzxcvbnm" + "qwertyuiopasdfghjklzxcvbnm".ToUpper()).Contains(ch);
    public static bool IsRusChar(char ch) => ("אבגדהזו¸זחיטךכלםןמנסעףפץצקרשתûü‎‏ÿ" + "אבגדהזו¸זחיטךכלםןמנסעףפץצקרשתûü‎‏ÿ".ToUpper()).Contains(ch);
    public static List<string> SplitWords(this string text)
    {
        List<string> words = new List<string>();

        string cur = "";
        bool first = true;
        string lang = "RU";
        foreach (char ch in text)
        {
            if( first == true)
            {
                lang = IsEngChar(ch)? "ENG": IsRusChar(ch)? "RU": "?";
                if(lang != "?")
                {
                    cur = (ch + "");
                    first = false;
                }
            }
            else
            { 
                string curlang = IsEngChar(ch) ? "ENG" : IsRusChar(ch) ? "RU" : "?";
                if(lang == curlang)
                {
                    cur += ch;
                }
                else
                {
                    words.Add(cur.ToUpper());
                    cur = "";
                    first = true;
                }
            }
        }
        if(cur != "")
        {
            words.Add(cur.ToUpper());
        }

        foreach(var word in words)
        {
            foreach(var ch in word)
            {
                if(IsEngChar(ch)==false && IsRusChar(ch) == false)
                {
                    throw new System.Exception("Ïאנסטל םוךמננוךעםמ: \n"+ text);
                }
            }
        }
        return words;
    }
    
}