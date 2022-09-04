using Newtonsoft.Json;

using System.Collections.Generic;
using System.Threading.Tasks;

using static MDB;


/// <summary>
/// 
/// </summary>
public class MovieKeywordsResponse
{
    public class MovieKeyword
    {
        public int id { get; set; }
        public string name { get; set; }
    }
    public int? id { get; set; }
    public List<MovieKeyword> keywords { get; set; }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}


/// <summary>
/// 
/// </summary>
public class MovieTrailersResponse
{
    public class MovieTrailer
    {
        public string size { get; set; }
        public string type { get; set; }
        public string source { get; set; }
        public string name { get; set; }
    }
    public int? id { get; set; }
    public List<MovieTrailer> quicktime { get; set; }
    public List<MovieTrailer> youtube { get; set; }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}



/// <summary>
/// 
/// </summary>
public interface IMDB
{

    Task<string> keywordSearch(string query, int page);
    Task<string> listSearch(string query, int page);
    Task<string> movieSearch(string query, int page);
    Task<string> personSearch(string query, int page);
    Task<string> tvSearch(string query, int page);


    Task<string> collectionSearch(string query, int page);
    Task<string> companySearch(string query, int page);
    Task<string> getCompany(int id);
    Task<string> getCompanyMovies(int id);
    Task<string> getMovieChanges();
    Task<MovieKeywordsResponse> getMovieKeywords(int id);
    Task<string> getMovieReleases(int id);
    Task<string> getMovieReviews(int id);
    Task<string> getMovieSimilarMovies(int id);
    Task<string> getMoviesLatest();
    Task<string> getMoviesPopular();
    Task<string> getMoviesTopRated();
    Task<MovieTrailersResponse> getMovieTrailers(int id);
    Task<string> getMovieTranslations(int id);
    Task<string> getPersonChanges();
    Task<string> getPersonChanges(int id);
    Task<string> getPersonExternalIds(int id);
    Task<string> getPersonImages(int id);
    Task<string> getPersonLatest();
    Task<string> getPersonPopular();
    Task<string> getPersonTaggedImages(int id);
    Task<string> getTvOnAiring();
    Task<string> getTvPopular();
    Task<string> getTvTodayAiring();
    Task<string> getTvTopRated();

}
 