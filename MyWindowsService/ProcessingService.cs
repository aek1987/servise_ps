public class ProcessingService
{
    public string ProcessPerson(Personne personne)
    {
        // Exemple de traitement : vérifier si la personne est majeure
        return personne.Age >= 18 ? $"{personne.Prenom} {personne.Nom} est majeur(e)." : $"{personne.Prenom} {personne.Nom} est mineur(e).";
    }
}
