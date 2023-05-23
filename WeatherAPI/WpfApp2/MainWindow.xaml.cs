using System.Threading.Tasks;
using System.Windows;
using System.Net.Http;
using Newtonsoft.Json;
using WpfApp2.Model;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        //Обработчик события нажатия на кнопку "Сформировать"
        private async void FormButton_Click(object sender, RoutedEventArgs e)
        {
            string city = CityTextBlock.Text;
            //Вызов асинхронного метода для получения информации о погоде
            await ConnectToApiService(city);
        }

        //Метод для выполнения API запроса и вывода информации о погоде
        private async Task ConnectToApiService(string city) 
        {
            string ApiKey = "4406607177093f68c16e59ec8fad8a19";

            string url = $"http://api.openweathermap.org/data/2.5/weather?q={city}&lang=ru&appid={ApiKey}";

            WeatherData weatherData;

            //Создание экземпляра HttpClient для выполнения запросов
            using (HttpClient client = new HttpClient()) 
            {

                try 
                {
                    // Отправка GET-запроса к API
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseText = await response.Content.ReadAsStringAsync();

                    // Десериализация полученных данных в объект WeatherData
                    weatherData = DeserializeJSON(responseText);

                    //Вывод информации на основе полученного ответа
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        WeatherIn.Text = $"Погода в {city}: ";

                        TemperatureTextBlock.Text = "Температура: " + (weatherData.Main.Temp-273.15).ToString("0.0") + " °C";
                        DescriptionTextBlock.Text = "Описание: " + weatherData.Weather[0].Description;
                        WindSpeedTextBlock.Text = "Скорость ветра: " + weatherData.Wind.Speed + " м/c";
                    });

                    //Очистка поля для ввода города
                    CityTextBlock.Text = null;
                }
                catch (HttpRequestException ex)
                {   //Вывод сообщения об ошибке в случае ошибки
                    MessageBox.Show($"Ошибка при выполнении запроса:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Метод для десериализации JSON-строки в объект WeatherData
        private WeatherData DeserializeJSON(string jsonData) 
        {
            return JsonConvert.DeserializeObject<WeatherData>(jsonData);
        }

    }
}
