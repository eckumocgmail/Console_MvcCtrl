﻿using System;

namespace ConsoleMDB
{
    public class Program
    {

        public static void Main(string[] args)
        {
            Run(args);
        }

        public static void Run(string[] args)
        {
            OnStart(args);
        }

        public static void OnStart(string[] args)
        {
            MDB api = new MDB();
            
            Console.WriteLine("\n\n\n" + api.getMovieTrailers(586097).Result);
            
            Console.WriteLine(api.companySearch("cinema",1 ).Result);  //=>companies: 60992,12299
            string json1 = api.getCompanyMovies(60992).Result;
            string json2 = api.getCompanyMovies(12299).Result;
            string json = api.getCompany(60992).Result;
            Console.WriteLine("\n\n\n"+json);
            Console.WriteLine("\n\n\n" + json1);
            Console.WriteLine("\n\n\n" + json2);

            //movies: 439981,442405,369925
            Console.WriteLine("\n\n\n" + api.getMovieReviews(439981).Result);
            Console.WriteLine("\n\n\n" + api.getMovieTranslations(442405).Result);
            Console.WriteLine("\n\n\n" + api.getMovieKeywords(369925).Result);
            Console.WriteLine("\n\n\n" + api.getMovieSimilarMovies(369925).Result);
            Console.WriteLine("\n\n\n" + api.getMoviesLatest().Result);
            Console.WriteLine("\n\n\n" + api.getMoviesPopular().Result);

            //1910848,208225
            Console.WriteLine("\n\n\n" + api.getPersonPopular().Result);
            Console.WriteLine("\n\n\n" + api.getPersonTaggedImages(1910848).Result);
            Console.WriteLine("\n\n\n" + api.getPersonImages(208225).Result);
            Console.WriteLine("\n\n\n" + api.getTvTopRated().Result);

        }
    }
}
