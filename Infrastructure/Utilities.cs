namespace Infrastructure;

public class Utilities
{
  private static Uri? Uri { get; set; }
  public static string FormttedConnectionString(string connectionString){
    Uri = new Uri(connectionString);
    return string.Format(
    "Server={0};Port={1};User Id={2};Password={3};Database={4};",
    Uri.Host,
    Uri.Port > 0 ? Uri.Port : "5432",
    Uri.UserInfo.Split(':')[0],
    Uri.UserInfo.Split(':')[1],
    Uri.AbsolutePath.Trim('/'));
  }
}
