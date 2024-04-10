using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Xml.Linq;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using static TCC_SAMMI.Presentation.Controller;

namespace TCC_SAMMI.Presentation
{
    public class Controller
    {
        //json to c# class https://json2csharp.com/
        public string pathuser = Path.Combine(Environment.CurrentDirectory, @"JSON\", "usuario.json");
        //public string pathusers= Path.Combine(Environment.CurrentDirectory, @"JSON\", "usuarios.json");

        private HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:7218"; // Assuming your API runs on localhost

        /*public void MyApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        public async Task<string> GetApiDataAsync()
        {
            string endpoint = "/api/usuario"; // Replace with your actual API endpoint
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                // Handle the error (e.g., log, throw exception)
                return null;
            }
        }*/

        public void MyApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        public async Task<Usuario> GetUsuario(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync("usuario/{id}");
            if (response.IsSuccessStatusCode)
            {
                string anwser = await response.Content.ReadAsStringAsync();
                Usuario user = Newtonsoft.Json.JsonConvert.DeserializeObject<Usuario>(anwser);
                return user;
            }
            else
            {
                // Handle the error (e.g., log, throw exception)
                return null;
            }
        }

        public async Task<Usuario> GetUsuariobyNome(string nome)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync($"https://localhost:7218/usuario/{nome}");
                if (response.IsSuccessStatusCode)
                {
                    string anwser = await response.Content.ReadAsStringAsync();
                    Usuario user = Newtonsoft.Json.JsonConvert.DeserializeObject<Usuario>(anwser);
                    return user;
                }
                else
                {
                    // Handle the error (e.g., log, throw exception)
                    return null;
                }
            }
        }

        public async Task<List<Usuario>> GetUsuarios()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("usuario");
            if (response.IsSuccessStatusCode)
            {
                string anwser = await response.Content.ReadAsStringAsync();
                List<Usuario> users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Usuario>>(anwser);
                return users;
            }
            else
            {
                // Handle the error (e.g., log, throw exception)
                return null;
            }
        }

        public async Task<bool> AddUsuario(Usuario data)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string jsonData = JsonConvert.SerializeObject(data);
                HttpContent httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync("https://localhost:7218/usuario", httpContent);
                Console.WriteLine(response);
                if (response.IsSuccessStatusCode)
                {
                    string anwser = await response.Content.ReadAsStringAsync();
                    return Convert.ToBoolean(anwser);
                }
                else
                {
                    // Handle the error (e.g., log, throw exception)
                    return false;
                }
            }
        }

        public async Task<string> EditaUsuario(string email, Usuario data)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string jsonData = JsonConvert.SerializeObject(data);
                HttpContent httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

                //var stringContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PutAsync($"https://localhost:7218/usuario/{email}", httpContent);
                Console.WriteLine(response);
                if (response.IsSuccessStatusCode)
                {
                    //after an user been edited, update the local json file
                    string anwser = await response.Content.ReadAsStringAsync();
                    Usuario user = Newtonsoft.Json.JsonConvert.DeserializeObject<Usuario>(anwser);

                    string json = JsonConvert.SerializeObject(user, Formatting.Indented);
                    System.IO.File.WriteAllText(pathuser, json);

                    return "";
                }
                else
                {
                    // Handle the error (e.g., log, throw exception)
                    return null;
                }
            }
        }

        public async Task<string> DeleteUsuario(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"usuario/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                // Handle the error (e.g., log, throw exception)
                return null;
            }
        }

        /*public void upadateusermatematica(UserMatematica newmatematica)
        {
            if (islogged())
            {
                Usuario usuario = getuser();
                usuario.matematica = newmatematica;

                string json = JsonConvert.SerializeObject(usuario, Formatting.Indented);
                System.IO.File.WriteAllText(pathuser, json);
            }
        }

        public void upadateuserportugues(UserPortugues newportu)
        {
            if (islogged())
            {
                Usuario usuario = getuser();
                usuario.portugues = newportu;

                string json = JsonConvert.SerializeObject(usuario, Formatting.Indented);
                System.IO.File.WriteAllText(pathuser, json);
            }
        }*/

        public bool islogged()
        {
            Usuario user = JsonConvert.DeserializeObject<Usuario>(File.ReadAllText(pathuser));
            return !string.IsNullOrEmpty(user.nome) ? true : false;
        }

        public dynamic getuser()
        {
           return JsonConvert.DeserializeObject<Usuario>(File.ReadAllText(pathuser));
        }

        /*public dynamic getusers()
        {
            string f = File.ReadAllText(pathusers);
            if (!string.IsNullOrEmpty(f)) { 
                return JsonConvert.DeserializeObject<List<Usuario>>(f);
            }
            else
            {
                List<Usuario> list = new List<Usuario> { };
                return list;
            }
        }*/

        public void logar(Usuario user)
        {
            string json = JsonConvert.SerializeObject(user, Formatting.Indented);
            System.IO.File.WriteAllText(pathuser, json);
        }

        /*public bool cadastrauser(IList<Usuario> list)
        {
            string json = JsonConvert.SerializeObject(list, Formatting.Indented);
            System.IO.File.WriteAllText(pathusers, json);
            deslogar();
            logar(list.Last());

            return true;
        }*/

        public void deslogar()
        {
            //clean the user json file
            Usuario u = new Usuario();
            string json = JsonConvert.SerializeObject(u, Formatting.Indented);
            System.IO.File.WriteAllText(pathuser, json);
        }
    }
}
