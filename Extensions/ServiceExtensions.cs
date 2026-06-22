public static class ServiceExtensions
{
    public static IServiceCollection AddServices (this IServiceCollection services)
    {
        services.AddScoped<ITraineeService,TraineeService>();
        services.AddScoped<IJWTService,JWTService>();
        services.AddScoped<IUserService,UserService>();
        services.AddScoped<IMentorService,MentorService>();
        services.AddScoped<ILearningTaskService,LearningTaskService>();
        services.AddScoped<ITaskAssignmentService,TaskAssignmentService>();
        services.AddScoped<ISubmissionService,SubmissionService>();
        services.AddScoped<IReviewService,ReviewService>();
        services.AddScoped<ILocalFileStorage,LocalFileStorage>();
        return services;
    } 
}