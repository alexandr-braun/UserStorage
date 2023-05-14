using System.Reflection;
using MediatR;
using UserStorage.Application;
using UserStorage.Presentation.Consumers.User;
using UserStorage.Presentation.Options;

namespace UserStorage.Presentation;

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
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        services.Configure<KafkaOptions>(
            Configuration.GetSection("Kafka"));

        services.AddMediatR(
            cfg=>cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly(), 
                Assembly.GetAssembly(typeof(IAssemblyMarker))));

        services.AddHostedService<UserKafkaBatchConsumer>();    
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

    }
}