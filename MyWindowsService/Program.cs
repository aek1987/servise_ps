using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;  // Assurez-vous que cette ligne est présente


using DZAEIDReader.Wrapper;


var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Ajout du service en arrière-plan
        services.AddHostedService<Worker>();

        // Ajout du service de traitement
        services.AddSingleton<ProcessingService>();

        // Ajout de l'API REST avec Kestrel
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    })
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseKestrel() // Utilisation de Kestrel pour héberger l'API
                  .Configure(app =>
                  {
                      // Activation du middleware de routage
                      app.UseRouting();

                      // Configuration des points d'API
                      app.UseEndpoints(endpoints =>
                      {


                          // Point d'API pour générer une Personne

                          endpoints.MapGet("/api/status", () =>
                          {
                              // Retourner un message de statut avec la date actuelle
                              return new
                              {
                                  status = "Service en ligne",
                                  date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                              };
                          });

                          // Point d'API pour générer une Personne

                          // Point d'API pour traiter une Personne
                          endpoints.MapGet("/api/read", () =>
                          {
                              var dllPath = Path.Combine(Directory.GetCurrentDirectory(), "lib", "DZAEIDReader.Wrapper.dll");

                              Console.WriteLine("Tentative de chargement de la DLL depuis : " + dllPath);
                              var assembly = System.Reflection.Assembly.LoadFrom(dllPath);

                              if (File.Exists(dllPath))
                              {
                                  Console.WriteLine("DLL trouvée à l'emplacement : " + dllPath);
                                  // Charger la DLL si nécessaire
                                  Wrappe reader = new Wrappe();
                                  int veriflicence = reader.VerifLicence("License.lic", "96587463-DGD2-2025-newv-123589647852");
                                  if (veriflicence == 0) {
                                      Console.WriteLine(" Citoyen nom :"+ Citoyen.NomFR);
                                  }
                                  else { Console.WriteLine("Licence invalide"); }

                              }
                              else
                              {
                                  Console.WriteLine("DLL introuvable à l'emplacement spécifié");
                              }

                          });



                          // Point d'API pour générer une Personne
                          endpoints.MapGet("/api/personne", () =>
                          {
                              // Génération d'une nouvelle instance de Personne
                              var personne = new Personne("Dupont", "Jean", 25);
                              return personne;
                          });

                          // Point d'API pour traiter une Personne
                          endpoints.MapGet("/api/personne/traiter", (ProcessingService processingService) =>
                          {
                              // Créer une instance de Personne à traiter
                              var personne = new Personne("Dupont", "Jean", 25);
                              var result = processingService.ProcessPerson(personne);
                              
                              // Retourner le résultat du traitement
                              return result;
                          });
                      });

                      // Activer Swagger pour une documentation API simple
                      app.UseSwagger();
                      app.UseSwaggerUI();
                  });
    });

var host = builder.Build();  // Construction du host
host.Run();  // Démarrage du service
