
using System;
using DZAEIDReader.Wrapper;
public class ProcessingService
{
   // Wrappe p;

    public string ProcessPerson(Personne personne)
    {
        // Exemple de traitement : vérifier si la personne est majeure
        return personne.Age >= 18 ? $"{personne.Prenom} {personne.Nom} est majeur(e)." : $"{personne.Prenom} {personne.Nom} est mineur(e).";
    }

    public void go()
    {
       

   /*     int result = p.VerifLicence("Lisence.lic", "96587463-DGD2-2025-newv-123589647852");
         Console.WriteLine("dlllllll ====== ");*/

   /*     if (result == 0)
        {
            int code = p.read_card("103039260", "870526", "270124", "");
            if (code != 0)
            {
                string erreur = p.Get_String_Erreur_Code(code);
                Console.WriteLine($"Erreur : {erreur}");
            }
            else
            {
                string nom = DZAEIDReader.Wrapper.Citoyen.NomFR;
                Console.WriteLine($"Nom : {nom}");
            }
        }
        else
        {
            Console.WriteLine("Licence invalide");
        }*/
    }



}
