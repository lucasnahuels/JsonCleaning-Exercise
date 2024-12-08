// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

Console.WriteLine("Hello, World!");
HttpClient client = new HttpClient();
string s = await client.GetStringAsync("https://coderbyte.com/api/challenges/json/json-cleaning");
RemoveInvalidValues(s);


static string RemoveInvalidValues(string input)
{
    var inputObject = JsonConvert.DeserializeObject<dynamic>(input);
    var validOutputList = new Dictionary<string, dynamic>();
    foreach (var item in inputObject)
    {
        var interiorValidOutputList = new Dictionary<string, string>();
        var itemKey = item.Path;
        var itemValue = item.Value;
        if (itemValue.Type is Newtonsoft.Json.Linq.JTokenType.Integer)
        {
            validOutputList.Add(itemKey, itemValue);
        }
        else
        {
            if (itemValue.Type == Newtonsoft.Json.Linq.JTokenType.String)
            {
                if (itemValue != "" && itemValue != "-" && itemValue != "N/A")
                {
                    validOutputList.Add(itemKey, itemValue);
                }
            }
            else if (itemValue is object)
            {
                if (itemValue.Type == Newtonsoft.Json.Linq.JTokenType.Array)
                {
                    var validOutputList2 = new List<string>();
                    foreach (var itemValue2 in itemValue)
                    {
                        if (itemValue2.ToString() != "" && itemValue2.ToString() != "-" && itemValue2.ToString() != "N/A")
                        {
                            validOutputList2.Add(itemValue2.ToString());
                        }
                    }
                    validOutputList.Add(itemKey, validOutputList2);
                }
                else
                {
                    if (((Newtonsoft.Json.Linq.JContainer)itemValue).Count > 1)
                    {
                        foreach (var item2 in itemValue)
                        {
                            var itemKey2 = item2.Name.ToString();
                            var itemValue2 = item2.Value.ToString();
                            if (itemValue2 != "" && itemValue2 != "-" && itemValue2 != "N/A")
                            {
                                interiorValidOutputList.Add(itemKey2, itemValue2);
                            }
                        }
                        validOutputList.Add(itemKey, interiorValidOutputList);
                    }
                }
            }
        }
    }
    return JsonConvert.SerializeObject(validOutputList);
}
