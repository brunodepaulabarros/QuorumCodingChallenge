using Microsoft.OpenApi.Models;
using QuorumCodingChallenge.Application.Services.BillServices;
using QuorumCodingChallenge.Domain.Repository;
using QuorumCodingChallenge.Infra.Extensions;
using QuorumCodingChallenge.Infra.Repository;

namespace QuorumCodingChallenge
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "QuorumCodingChallenge", Version = "v1" });
            });

            #region Denepency Injection
            services.AddScoped<IBillService, BillService>();

            services.AddScoped<IPersonRepository,PersonRepository>();
            services.AddScoped<IVoteRepository, VoteRepository>();
            services.AddScoped<IVoteResultRepository, VoteResultRepository>();
            services.AddScoped<IBillRepository, BillRepository>();
            services.AddScoped<CsvHelperExtension, CsvHelperExtension>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "QuorumCodingChallenge v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
