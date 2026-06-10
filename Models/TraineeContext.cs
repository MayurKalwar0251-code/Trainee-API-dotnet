using Microsoft.EntityFrameworkCore;

namespace TrainineeAPI.Models;

public class TraineeContext : DbContext
{
    public TraineeContext(DbContextOptions<TraineeContext> options) : base(options)
    {
        
    }

    public DbSet<Trainee> Trainees {get; set;} = null!;

    public DbSet<User> Users {get; set;} = null!;
    public DbSet<Mentor> Mentors {get; set;} = null!;
    public DbSet<LearningTask> LearningTasks {get; set;} = null!;
    public DbSet<TaskAssignment> TaskAssignments {get; set;} = null!;
    public DbSet<Submission> Submissions {get; set;} = null!;
    public DbSet<Review> Reviews {get; set;} = null!;
}