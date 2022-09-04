using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

 
public class MDB : IMDB
{
    private static string url = "http://api.themoviedb.org/3";
    public static string apiKey = "72b56103e43843412a992a8d64bf96e9";

    //"authentication/session/new?api_key=72b56103e43843412a992a8d64bf96e9&request_token"+requestToken
    //"authentication/guest_session/new?api_key=72b56103e43843412a992a8d64bf96e9"
    //"authentication/token/new?api_key=72b56103e43843412a992a8d64bf96e9"


    public async Task<string> getTvOnAiring()
    {
        string query = url + "/tv/on_the_air?api_key=72b56103e43843412a992a8d64bf96e9";
        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        HttpResponseMessage response = await client.GetAsync(query);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
    public async Task<string> getTvTodayAiring()
    {
        string query = url + "/tv/airing_today?api_key=72b56103e43843412a992a8d64bf96e9";
        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        HttpResponseMessage response = await client.GetAsync(query);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
    public async Task<string> getTvTopRated()
    {
        string query = url + "/tv/top_rated?api_key=72b56103e43843412a992a8d64bf96e9";
        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        HttpResponseMessage response = await client.GetAsync(query);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
    public async Task<string> getTvPopular()
    {
        string query = url + "/tv/popular?api_key=72b56103e43843412a992a8d64bf96e9";
        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        HttpResponseMessage response = await client.GetAsync(query);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }




    public async Task<string> getPersonExternalIds(int id)
    {
        string query = url + "/person/" + id + "/external_ids?api_key=72b56103e43843412a992a8d64bf96e9";
        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        HttpResponseMessage response = await client.GetAsync(query);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
    public async Task<string> getPersonImages(int id)
    {
        string query = url + "/person/" + id + "/images?api_key=72b56103e43843412a992a8d64bf96e9";
        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        HttpResponseMessage response = await client.GetAsync(query);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
    public async Task<string> getPersonTaggedImages(int id)
    {
        string query = url + "/person/" + id + "/tagged_images?api_key=72b56103e43843412a992a8d64bf96e9";
        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        HttpResponseMessage response = await client.GetAsync(query);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
    public async Task<string> getPersonChanges(int id)
    {
        string query = url + "/person/" + id + "/changes?api_key=72b56103e43843412a992a8d64bf96e9";
        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        HttpResponseMessage response = await client.GetAsync(query);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
    public async Task<string> getPersonPopular()
    {
        string query = url + "/person/popular?api_key=72b56103e43843412a992a8d64bf96e9";
        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        HttpResponseMessage response = await client.GetAsync(query);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
    public async Task<string> getPersonLatest()
    {
        string query = url + "/person/latest?api_key=72b56103e43843412a992a8d64bf96e9";
        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        HttpResponseMessage response = await client.GetAsync(query);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
    public async Task<string> getMovieChanges()
    {
        string query = url + "/movie/changes?api_key=72b56103e43843412a992a8d64bf96e9";
        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        HttpResponseMessage response = await client.GetAsync(query);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> getCompany(int id)
    {

        string query = url + "/company/" + id + "?api_key=72b56103e43843412a992a8d64bf96e9";
        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        HttpResponseMessage response = await client.GetAsync(query);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }


    public async Task<string> getCompanyMovies(int id)
    {
        string query = url + "/company/" + id + "/movies?api_key=72b56103e43843412a992a8d64bf96e9";
        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        HttpResponseMessage response = await client.GetAsync(query);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    

    //{"id":2778,"keywords":[{"id":4358,"name":"shenanigan"},{"id":6464,"name":"uncle"}]}
    public async Task<MovieKeywordsResponse> getMovieKeywords(int id)
    {
        string query = url + "/movie/" + id + "/keywords?api_key=72b56103e43843412a992a8d64bf96e9";
        Console.WriteLine(query);
        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        HttpResponseMessage response = await client.GetAsync(query);
        response.EnsureSuccessStatusCode();
        string json = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject< MovieKeywordsResponse>(json);

    }

    public async Task<string> getMovieTranslations(int id)
    {
        string query = url + "/movie/" + id + "/translations?api_key=72b56103e43843412a992a8d64bf96e9";
        Console.WriteLine(query);
        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        HttpResponseMessage response = await client.GetAsync(query);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();

    }

    public async Task<string> getMovieSimilarMovies(int id)
    {
        string query = url + "/movie/" + id + "/similar_movies?api_key=72b56103e43843412a992a8d64bf96e9";
        Console.WriteLine(query);
        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        HttpResponseMessage response = await client.GetAsync(query);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> getMoviesTopRated()
    {
        string query = url + "/movie/top_rated?api_key=72b56103e43843412a992a8d64bf96e9";
        Console.WriteLine(query);
        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        HttpResponseMessage response = await client.GetAsync(query);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> getMoviesPopular()
    {
        string query = url + "/movie/popular?api_key=72b56103e43843412a992a8d64bf96e9";
        Console.WriteLine(query);
        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        HttpResponseMessage response = await client.GetAsync(query);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> getMoviesLatest()
    {
        string query = url + "/movie/latest?api_key=72b56103e43843412a992a8d64bf96e9";
        Console.WriteLine(query);
        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        HttpResponseMessage response = await client.GetAsync(query);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> getMovieReviews(int id)
    {
        string query = url + "/movie/" + id + "/reviews?api_key=72b56103e43843412a992a8d64bf96e9";
        Console.WriteLine(query);
        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        HttpResponseMessage response = await client.GetAsync(query);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<MovieTrailersResponse> getMovieTrailers(int id)
    {
        string query = url + "/movie/" + id + "/trailers?api_key=72b56103e43843412a992a8d64bf96e9";
        Console.WriteLine(query);
        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        HttpResponseMessage response = await client.GetAsync(query);
        response.EnsureSuccessStatusCode();
        
        string json = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<MovieTrailersResponse>(json);

    }

    public async Task<string> getMovieReleases(int id)
    {
        string query = url + "/movie/" + id + "/releases?api_key=72b56103e43843412a992a8d64bf96e9";
        Console.WriteLine(query);
        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        HttpResponseMessage response = await client.GetAsync(query);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();

    }



    public async Task<string> getPersonChanges()
    {
        string query = url + "/person/changes?api_key=72b56103e43843412a992a8d64bf96e9";
        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        HttpResponseMessage response = await client.GetAsync(query);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }






    public async Task<string> keywordSearch(string query, int page)
    {
        return await this.searchAsync("keyword", query, page);
    }

    public async Task<string> collectionSearch(string query, int page)
    {
        return await this.searchAsync("collection", query, page);
    }

    public async Task<string> personSearch(string query, int page)
    {
        return await this.searchAsync("person", query, page);
    }

    public async Task<string> movieSearch(string query, int page)
    {
        return await this.searchAsync("movie", query, page);
    }

    public async Task<string> tvSearch(string query, int page)
    {
        return await this.searchAsync("tv", query, page);
    }

    public async Task<string> companySearch(string query, int page)
    {
        return await this.searchAsync("company", query, page);
    }

    public async Task<string> listSearch(string query, int page)
    {
        return await this.searchAsync("list", query, page);
    }

    protected async Task<string> searchAsync(string category, string query, int page)
    {
        Dictionary<string, string> queryParams = new Dictionary<string, string>();
        queryParams["query"] = query;
        queryParams["page"] = page.ToString();
        queryParams["api_key"] = apiKey;
        return await this.request(queryParams, "search", category);
    }

    private async Task<string> request(Dictionary<string, string> queryParams, string action, string category)
    {
        string query = url + "/" + action + "/" + category + "?";

        foreach (var pair in queryParams)
        {
            query += $"{pair.Key}={pair.Value}&";
        }
        query = query.Substring(0, query.Length - 1);
        Console.WriteLine(query);

        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        HttpResponseMessage response = await client.GetAsync(query);



        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }


}
