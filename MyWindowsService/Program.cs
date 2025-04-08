using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;

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
