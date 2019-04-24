using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TOM.Core.Entities;


namespace TOM.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly VooWs.VooWSSoapClient _vooClient;
        private readonly PassagemWs.PassagemWSSoapClient _passegmClient;

        public HomeController()
        {
            _vooClient = new VooWs.VooWSSoapClient();
            _passegmClient = new PassagemWs.PassagemWSSoapClient();
        }

        public ActionResult Index()
        {
            //var result = _vooClient.BuscarTodos();

            using (var client = new VooWs.VooWSSoapClient())
            {
                var result = client.BuscarTodos();

                List<Voo> voos = new List<Voo>();
                foreach (var item in result)
                {
                    var voo = new Voo()
                    {
                        Id = item.Id,
                        Aeronave = item.Aeronave,
                        DataVoo = item.DataVoo,
                        NumeroVoo = item.NumeroVoo,
                        Origem = item.Origem,
                        Destino = item.Destino,
                        Passagens = GetPassagens(item.Id),
                        ValorUnicoPassagem = item.ValorUnicoPassagem,
                        QuantidadeAssentos = item.QuantidadeAssentos
                    };

                    voo.QuantidadeAssentosLivres = voo.RetornarQuantidadeLugaresLivres();

                    voos.Add(voo);
                }

                return View(voos);
            }

            //return View(result);
        }

        private IList<Passagem> GetPassagens(int id)
        {
            var passagensWs = _passegmClient.BuscarPorVoo(id);

            var passagens = new List<Passagem>();

            foreach (var passagem in passagensWs)
            {
                passagens.Add(
                    new Passagem()
                    {
                        Id = passagem.Id,
                        DataVoo = passagem.DataVoo,
                        ValorPassagem = passagem.ValorPassagem,
                    }
                );
            }

            return passagens;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult ComprarBilhete(int idVoo)
        {
            _passegmClient.ComprarBilhete(idVoo);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Voo voo)
        {
            _vooClient.Criar(
                new VooWs.Voo()
                {
                    Id = voo.Id,
                    Aeronave = voo.Aeronave,
                    DataVoo = voo.DataVoo,
                    Destino = voo.Destino,
                    NumeroVoo = voo.NumeroVoo,
                    Origem = voo.Origem,
                    QuantidadeAssentos = voo.QuantidadeAssentos,
                    ValorUnicoPassagem = voo.ValorUnicoPassagem
                }

                );

            return RedirectToAction("Index");
        }
    }
}