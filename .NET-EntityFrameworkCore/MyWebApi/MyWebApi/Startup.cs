﻿namespace MyWebApi.Web
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.OpenApi.Models;
    using MyWebApi.Data;
    using MyWebApi.Data.Repositories;
    using MyWebApi.Business.Services;
    using System.Reflection;

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

            services.AddDbContext<MyWebApiDbContext>(options => options
               .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
               .LogTo(message => File.AppendAllText(
                   Path.Combine(Assembly.GetExecutingAssembly()
                   .Location, @"..\..\..\..\..\logs.txt"), message))
           );

            services.AddScoped<DbInitializer>();

            services.AddScoped<IToDoRepository, ToDoRepository>();
            services.AddScoped<IToDoService, ToDoService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyWebApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyWebApi v1"));
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
