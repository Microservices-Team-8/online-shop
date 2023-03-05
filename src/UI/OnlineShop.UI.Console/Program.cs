
const string url = "http://localhost:8080/api/";

HttpClient httpClient = new HttpClient();

Console.WriteLine("-------------------------");
Console.WriteLine("\nUser endpoint:\n");
var res1 = await httpClient.GetStringAsync(url + "user");
Console.WriteLine(res1);

Console.WriteLine("\nStore endpoint:\n");
var res2 = await httpClient.GetStringAsync(url + "store");
Console.WriteLine(res2);

Console.WriteLine("\nBasket endpoint:\n");
var res3 = await httpClient.GetStringAsync(url + "basket");
Console.WriteLine(res3);

Console.WriteLine("\nOrder endpoint:\n");
var res4 = await httpClient.GetStringAsync(url + "order");
Console.WriteLine(res4);

Console.WriteLine("-------------------------");