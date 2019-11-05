using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DigikabuWPF
{
    class WebsiteCon
    {
        static string UN { get; set; }
        static string PW { get; set; }

        static HttpClient client = new HttpClient(new HttpClientHandler
        {
            AllowAutoRedirect = true,
            UseCookies = true,
            CookieContainer = new CookieContainer()
        });
        public static async Task<string> Einloggen(string username, string passwort)
        {
            UN = username;
            PW = passwort;
            try
            {
                var values = new Dictionary<string, string>
                 {
                 { "UserName", UN },
                 { "Password", PW }
                 };

                FormUrlEncodedContent content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("https://digikabu.de/Login/Proceed", content);

                var responseString = await response.Content.ReadAsStringAsync();


                if (responseString.Contains("Falscher Benutzername"))
                {
                    return "Falscher Benutzername oder Passwort!";
                }
                else
                {
                    return "Erfolgreich eingeloggt!";
                }
            }
            catch (Exception)
            {
                return "Konnte nicht mit server verbinden!";


            }
        }
        public static async Task<List<string>> getTermine()
        {
            List<string> Termine = new List<string>();
            try
            {
                relog();
                var response = await client.GetAsync("https://digikabu.de/Main");

                var responseString = await response.Content.ReadAsStringAsync();
                string[] info = new string[2];
                bool nextIsIgnore = false, nextIsMessage = false;
               
                foreach (string s in responseString.Split('<'))
                {

                    if (s.Contains("td"))
                    {
                        var trim = s.Trim();
                        if (trim.Contains("white-space"))
                        {
                            var x = trim.Split('>');
                            info[0] = fix(x[1]) + ": ";
                            nextIsIgnore = true;
                        }
                        else if (nextIsIgnore)
                        {
                            nextIsMessage = true;
                            nextIsIgnore = false;
                        }
                        else if (nextIsMessage)
                        {
                            var x = trim.Split('>');
                            nextIsMessage = false;
                            info[1] = fix(x[1]);
                            Termine.Add(info[0] + info[1]);

                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return Termine;
        }
        public static async Task<List<string>> getStundenplan()
        {
            List<string> stundenl = new List<string>();
            try
            {
                relog();
                var response = await client.GetAsync("https://digikabu.de/Main");
                var responsestring = await response.Content.ReadAsStringAsync();
                string[] stunden = new string[10];
                stunden[0] = "1. Stunde  (8:30 - 9:15)      : ";
                stunden[1] = "2. Stunde  (9:15 - 10:00)    : ";
                stunden[2] = "3. Stunde  (10:15 - 11:00)  : ";
                stunden[3] = "4. Stunde  (11:00 - 11:45)  : ";
                stunden[4] = "5. Stunde  (11:45 - 12:30)  : ";
                stunden[5] = "6. Stunde  (12:30 - 13:15)  : ";
                stunden[6] = "7. Stunde  (13:15 - 14:00)  : ";
                stunden[7] = "8. Stunde  (14:00 - 14:45)  : ";
                stunden[8] = "9. Stunde  (14:45 - 15:30)  : ";
                stunden[9] = "10. Stunde (15:30 - 16:15) : ";
                int fach = 0;
                int fach2 = 0;
                string[] splitfach1 = new string[] { };
                string fachsave = string.Empty;
                foreach (string s in responsestring.Split('<'))
                {

                    if (s.Contains("svg x="))
                    {
                        string[] split = s.Split(' ');
                        string[] fach1 = split[2].Split('\'');
                        string[] fachx = split[4].Split('\'');
                        splitfach1 = split[1].Split('\'');
                        //splitfach2 = split[3].Split('\'');
                        fach2 = Convert.ToInt32(fachx[1]) / 60;
                        fach = Convert.ToInt32(fach1[1]) / 60;
                    }

                    if (s.Contains("text-anchor='middle'"))
                    {
                        string[] split = s.Split('>');
                        fachsave = split[1];

                        if (fach2 < 2)
                        {
                            if (splitfach1[1] != "115")
                            {
                                stunden[fach] += fachsave;
                            }
                            else
                            {
                                stunden[fach] += " / " + fachsave;
                            }
                        }
                        else
                        {
                            if (fach2 == 2)
                            {
                                switch (fach)
                                {
                                    case 0:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[0] += fachsave;
                                            stunden[1] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[0] += " / " + fachsave;
                                            stunden[1] += " / " + fachsave;
                                        }

                                        break;
                                    case 1:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[1] += fachsave;
                                            stunden[2] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[1] += " / " + fachsave;
                                            stunden[2] += " / " + fachsave;
                                        }
                                        break;
                                    case 2:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[2] += fachsave;
                                            stunden[3] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[2] += " / " + fachsave;
                                            stunden[3] += " / " + fachsave;
                                        }
                                        break;
                                    case 3:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[3] += fachsave;
                                            stunden[4] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[3] += " / " + fachsave;
                                            stunden[4] += " / " + fachsave;
                                        }
                                        break;
                                    case 4:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[4] += fachsave;
                                            stunden[5] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[4] += " / " + fachsave;
                                            stunden[5] += " / " + fachsave;
                                        }
                                        break;
                                    case 5:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[5] += fachsave;
                                            stunden[6] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[5] += " / " + fachsave;
                                            stunden[6] += " / " + fachsave;
                                        }
                                        break;
                                    case 6:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[6] = fachsave;
                                            stunden[7] = fachsave;
                                        }
                                        else
                                        {
                                            stunden[6] += " / " + fachsave;
                                            stunden[7] += " / " + fachsave;
                                        }
                                        break;
                                    case 7:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[7] += fachsave;
                                            stunden[8] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[7] += " / " + fachsave;
                                            stunden[8] += " / " + fachsave;
                                        }
                                        break;
                                    case 8:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[8] += fachsave;
                                            stunden[9] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[8] += " / " + fachsave;
                                            stunden[9] += " / " + fachsave;
                                        }
                                        break;
                                    case 9:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[9] += fachsave;

                                        }
                                        else
                                        {
                                            stunden[9] += " / " + fachsave;

                                        }
                                        break;
                                }
                            }
                            else
                            {
                                switch (fach)
                                {
                                    case 0:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[0] += fachsave;
                                            stunden[1] += fachsave;
                                            stunden[2] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[0] += " / " + fachsave;
                                            stunden[1] += " / " + fachsave;
                                            stunden[2] += " / " + fachsave;
                                        }
                                        break;
                                    case 1:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[1] += fachsave;
                                            stunden[2] += fachsave;
                                            stunden[3] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[1] += " / " + fachsave;
                                            stunden[2] += " / " + fachsave;
                                            stunden[3] += " / " + fachsave;
                                        }
                                        break;
                                    case 2:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[2] += fachsave;
                                            stunden[3] += fachsave;
                                            stunden[4] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[2] += " / " + fachsave;
                                            stunden[3] += " / " + fachsave;
                                            stunden[4] += " / " + fachsave;
                                        }
                                        break;
                                    case 3:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[3] += fachsave;
                                            stunden[4] += fachsave;
                                            stunden[5] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[3] += " / " + fachsave;
                                            stunden[4] += " / " + fachsave;
                                            stunden[5] += " / " + fachsave;
                                        }
                                        break;
                                    case 4:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[4] += fachsave;
                                            stunden[5] += fachsave;
                                            stunden[6] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[4] += " / " + fachsave;
                                            stunden[5] += " / " + fachsave;
                                            stunden[6] += " / " + fachsave;
                                        }
                                        break;
                                    case 5:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[5] += fachsave;
                                            stunden[6] += fachsave;
                                            stunden[7] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[5] += " / " + fachsave;
                                            stunden[6] += " / " + fachsave;
                                            stunden[7] += " / " + fachsave;
                                        }
                                        break;
                                    case 6:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[6] += fachsave;
                                            stunden[7] += fachsave;
                                            stunden[8] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[6] += " / " + fachsave;
                                            stunden[7] += " / " + fachsave;
                                            stunden[8] += " / " + fachsave;
                                        }
                                        break;
                                    case 7:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[7] += fachsave;
                                            stunden[8] += fachsave;
                                            stunden[9] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[7] += " / " + fachsave;
                                            stunden[8] += " / " + fachsave;
                                            stunden[9] += " / " + fachsave;
                                        }
                                        break;
                                    case 8:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[8] += fachsave;
                                            stunden[9] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[8] += " / " + fachsave;
                                            stunden[9] += " / " + fachsave;
                                        }
                                        break;
                                    case 9:
                                        if (splitfach1[1] != "115")
                                        {

                                            stunden[9] += fachsave;
                                        }
                                        else
                                        {

                                            stunden[9] += " / " + fachsave;
                                        }
                                        break;
                                }
                            }

                        }
                    }

                }
                foreach (string s in stunden)
                {
                    string edit_s = s;
                    if (edit_s == null)
                    {
                        edit_s = "";
                    }
                    if (s.Contains("3. Stunde"))
                    {
                        stundenl.Add("Pause (10:00 - 10:15)");
                    }
                    //TODO ADD RAUM
                    stundenl.Add(edit_s);
                    
                    
                }
            }
            catch (Exception)
            {

            }
            return stundenl;
        }
        public static async Task<List<string>> getStundenplan(DateTime date)
        {
            List<string> stundenl = new List<string>();
            try
            {
                relog();
                var response = await client.GetAsync("https://digikabu.de/Main?date=" + date.ToString("yyyy-MM-dd"));
                var responsestring = await response.Content.ReadAsStringAsync();
                string[] stunden = new string[10];
                int fach = 0;
                int fach2 = 0;
                string[] splitfach1 = new string[] { };
                string fachsave = string.Empty;
                foreach (string s in responsestring.Split('<'))
                {

                    if (s.Contains("svg x="))
                    {
                        string[] split = s.Split(' ');
                        string[] fach1 = split[2].Split('\'');
                        string[] fachx = split[4].Split('\'');
                        splitfach1 = split[1].Split('\'');
                        //splitfach2 = split[3].Split('\'');
                        fach2 = Convert.ToInt32(fachx[1]) / 60;
                        fach = Convert.ToInt32(fach1[1]) / 60;
                    }

                    if (s.Contains("text-anchor='middle'"))
                    {
                        string[] split = s.Split('>');
                        fachsave = split[1];

                        if (fach2 < 2)
                        {
                            if (splitfach1[1] != "115")
                            {
                                stunden[fach] += fachsave;
                            }
                            else
                            {
                                stunden[fach] += " / " + fachsave;
                            }
                        }
                        else
                        {
                            if (fach2 == 2)
                            {
                                switch (fach)
                                {
                                    case 0:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[0] += fachsave;
                                            stunden[1] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[0] += " / " + fachsave;
                                            stunden[1] += " / " + fachsave;
                                        }

                                        break;
                                    case 1:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[1] += fachsave;
                                            stunden[2] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[1] += " / " + fachsave;
                                            stunden[2] += " / " + fachsave;
                                        }
                                        break;
                                    case 2:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[2] += fachsave;
                                            stunden[3] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[2] += " / " + fachsave;
                                            stunden[3] += " / " + fachsave;
                                        }
                                        break;
                                    case 3:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[3] += fachsave;
                                            stunden[4] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[3] += " / " + fachsave;
                                            stunden[4] += " / " + fachsave;
                                        }
                                        break;
                                    case 4:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[4] += fachsave;
                                            stunden[5] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[4] += " / " + fachsave;
                                            stunden[5] += " / " + fachsave;
                                        }
                                        break;
                                    case 5:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[5] += fachsave;
                                            stunden[6] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[5] += " / " + fachsave;
                                            stunden[6] += " / " + fachsave;
                                        }
                                        break;
                                    case 6:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[6] = fachsave;
                                            stunden[7] = fachsave;
                                        }
                                        else
                                        {
                                            stunden[6] += " / " + fachsave;
                                            stunden[7] += " / " + fachsave;
                                        }
                                        break;
                                    case 7:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[7] += fachsave;
                                            stunden[8] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[7] += " / " + fachsave;
                                            stunden[8] += " / " + fachsave;
                                        }
                                        break;
                                    case 8:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[8] += fachsave;
                                            stunden[9] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[8] += " / " + fachsave;
                                            stunden[9] += " / " + fachsave;
                                        }
                                        break;
                                    case 9:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[9] += fachsave;

                                        }
                                        else
                                        {
                                            stunden[9] += " / " + fachsave;

                                        }
                                        break;
                                }
                            }
                            else
                            {
                                switch (fach)
                                {
                                    case 0:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[0] += fachsave;
                                            stunden[1] += fachsave;
                                            stunden[2] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[0] += " / " + fachsave;
                                            stunden[1] += " / " + fachsave;
                                            stunden[2] += " / " + fachsave;
                                        }
                                        break;
                                    case 1:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[1] += fachsave;
                                            stunden[2] += fachsave;
                                            stunden[3] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[1] += " / " + fachsave;
                                            stunden[2] += " / " + fachsave;
                                            stunden[3] += " / " + fachsave;
                                        }
                                        break;
                                    case 2:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[2] += fachsave;
                                            stunden[3] += fachsave;
                                            stunden[4] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[2] += " / " + fachsave;
                                            stunden[3] += " / " + fachsave;
                                            stunden[4] += " / " + fachsave;
                                        }
                                        break;
                                    case 3:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[3] += fachsave;
                                            stunden[4] += fachsave;
                                            stunden[5] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[3] += " / " + fachsave;
                                            stunden[4] += " / " + fachsave;
                                            stunden[5] += " / " + fachsave;
                                        }
                                        break;
                                    case 4:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[4] += fachsave;
                                            stunden[5] += fachsave;
                                            stunden[6] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[4] += " / " + fachsave;
                                            stunden[5] += " / " + fachsave;
                                            stunden[6] += " / " + fachsave;
                                        }
                                        break;
                                    case 5:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[5] += fachsave;
                                            stunden[6] += fachsave;
                                            stunden[7] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[5] += " / " + fachsave;
                                            stunden[6] += " / " + fachsave;
                                            stunden[7] += " / " + fachsave;
                                        }
                                        break;
                                    case 6:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[6] += fachsave;
                                            stunden[7] += fachsave;
                                            stunden[8] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[6] += " / " + fachsave;
                                            stunden[7] += " / " + fachsave;
                                            stunden[8] += " / " + fachsave;
                                        }
                                        break;
                                    case 7:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[7] += fachsave;
                                            stunden[8] += fachsave;
                                            stunden[9] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[7] += " / " + fachsave;
                                            stunden[8] += " / " + fachsave;
                                            stunden[9] += " / " + fachsave;
                                        }
                                        break;
                                    case 8:
                                        if (splitfach1[1] != "115")
                                        {
                                            stunden[8] += fachsave;
                                            stunden[9] += fachsave;
                                        }
                                        else
                                        {
                                            stunden[8] += " / " + fachsave;
                                            stunden[9] += " / " + fachsave;
                                        }
                                        break;
                                    case 9:
                                        if (splitfach1[1] != "115")
                                        {

                                            stunden[9] += fachsave;
                                        }
                                        else
                                        {

                                            stunden[9] += " / " + fachsave;
                                        }
                                        break;
                                }
                            }

                        }
                    }

                }
                foreach (string s in stunden)
                {
                    string edit_s = s;
                    if (edit_s == null)
                    {
                        edit_s = "";
                    }
                    //TODO ADD RAUM
                    stundenl.Add(edit_s);


                }
            }
            catch (Exception)
            {

            }
            return stundenl;
        }
        private static async void relog()
        {
            var values = new Dictionary<string, string>
                 {
                 { "UserName", UN },
                 { "Password", PW }
                 };

            FormUrlEncodedContent content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("https://digikabu.de/Login/Proceed", content);

            var responseString = await response.Content.ReadAsStringAsync();
        }
        public static async Task<string> GetUNKL()
        {
            relog();
            string retval = string.Empty;
            var response = await client.GetAsync("https://digikabu.de/Main");

            var responseString = await response.Content.ReadAsStringAsync();
            if (!responseString.Contains("<input class=\"form - control\" type=\"password\" id=\"Password\" name=\"Password\" />"))
            {
                foreach (string s in responseString.Split('>'))
                {
                    string klasse = string.Empty;

                    if (s.Contains(")</span"))
                    {
                        string[] split = s.Split(' ');
                        retval = fix(split[0]) + " " + fix(split[1]);
                        foreach(string st in split)
                        {
                            if (st.Contains('('))
                            {
                                klasse = st.Trim(new char[] { '(', ')' });
                            }
                        }
                        
                        string[] klassesplit = klasse.Split(')');
                        retval +=";"+klassesplit[0];

                    }

                }
                
            }
            return retval;
        }
        public static async void logout()
        {
            relog();
            try
            {
                var response = await client.GetAsync("https://digikabu.de/Logout");

                var responseString = await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {

            }
        }
        static string fix(string toFix)
        {
            string ret = string.Empty;
            if (toFix.Contains("&#x"))
            {
                ret = toFix;
                ret = ret.Replace("&#xFC;", "ü");
                ret = ret.Replace("&#xDF;", "ß");
                ret = ret.Replace("&#xF6;", "ö");
                ret = ret.Replace("&#xE4;", "ä");
            }
            else
            {
                ret = toFix;
            }
            return ret;
        }
    }
}
