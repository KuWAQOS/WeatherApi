using System.Collections.Generic;

namespace WpfApp2.Model
{
    // Класс, представляющий данные о погоде
    internal class WeatherData
    {
        public List<Weather> Weather { get; set; }
        public MainData Main { get; set; }
        public WindData Wind { get; set; }

    }
}
