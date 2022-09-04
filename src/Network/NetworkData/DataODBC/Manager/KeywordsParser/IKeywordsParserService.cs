namespace KeywordsPW
{
   using System;
   using System.Collections.Generic;

   public interface IKeywordsParserService
   {
      public IDictionary<string, int> ParseKeywords(string Resource);
   }

}