using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSearch
{

    public interface ISearchableResource
    {
        public string GetUrl();
        public IDictionary<string, long> GetKeywords();
    }


    public interface ISearch
    {
        public IEnumerable<ISearchableResource> Search(string query);

        public IDictionary<ISearchableResource, long> PreSearch(IEnumerable<string> keywords);


        IEnumerable<string> ParseQuery(string query);
        
    }

    public interface IInternet
    {
        public IDictionary<string, ISearchableResource> GetResources();
    }


    
    public class Search : ISearch, IComparer<KeyValuePair<ISearchableResource, long>>
    {
        private readonly IInternet _internet;

        public Search(IInternet internet)
        {
            _internet = internet;
        }

        public int Compare(KeyValuePair<ISearchableResource,long> x, KeyValuePair<ISearchableResource, long> y)
        {
            return (int)(x.Value - y.Value);
        }

        public IEnumerable<string> ParseQuery(string query)
        {
            return query.Replace("  ", " ").Trim().ToLower().Split(" ");
        }

        public IDictionary<ISearchableResource, long> PreSearch(IEnumerable<string> words)
        {
            var result = new Dictionary<ISearchableResource, long>();
            foreach(var resource in _internet.GetResources().Values)
            {
                long rating = 0;
                resource.GetKeywords().Where(kv => words.Contains(kv.Key)).ToList().ForEach(kv=> rating+=kv.Value);
                result[resource] = rating;
            }
            return result;
        }

        IEnumerable<ISearchableResource> ISearch.Search(string query)
        {
            var words = this.ParseQuery(query);
            var results = this.PreSearch(words).ToList();
            results.Sort(this);
            return results.Select(s=>s.Key).Take(10);
        }
    }
}
