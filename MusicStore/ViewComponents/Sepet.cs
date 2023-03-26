using Microsoft.AspNetCore.Mvc;
using MusicStore.Data;
using MusicStore.Models;
using MusicStore.ViewModels;

namespace MusicStore.ViewComponents
{
    //[ViewComponent]
    public class Sepet : ViewComponent
    {
        //asp.net coreda kullanılmaya başlandı.
        //urlden direkt adını yazarak çağıramayız.
        //viewComponent/ (unity:prefabs)
        //farklı sayfalarda tekrar tekrar kullanılacak bir yapı oluşturmuş oluruz.



        //asp.net core öncesi 
        //partialview : bir view'ın bir kısmı. (loginpartial,cok satanlar,katerogiler vs.)
        //childActionOnly: bu urlden viewadı yazılınca çalışamaz anlamına gelir.
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // ViewComponent'iniz için verileri hazırlayın
            string deger=HttpContext.Session.GetString("adet");

            int sepetUrunAdedi;
            if (deger != null)
            {
                sepetUrunAdedi = int.Parse(deger);
            }
            else
            {
                sepetUrunAdedi = 0;
            }
            // Verileri bir ViewComponentResult nesnesine ekleyin ve döndürün
            return View(sepetUrunAdedi);
        }
    }
}
