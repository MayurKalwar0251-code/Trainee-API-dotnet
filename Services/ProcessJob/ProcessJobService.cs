using Microsoft.EntityFrameworkCore;
using TrainineeAPI.Models;

class ProcessJobService : IProcessJobService
{
    private readonly TraineeContext _traineeContext;
    private readonly IRabbitMQPublisher _rabbitMQPublisher;
    public ProcessJobService(TraineeContext traineeContext,IRabbitMQPublisher rabbitMQPublisher)
    {
        _traineeContext = traineeContext;
        _rabbitMQPublisher = rabbitMQPublisher;
    }
    public async Task<ServiceResult<ProcessingJob>> GetById(int id)
    {
        var processingJob = await _traineeContext.ProcessingJobs.FirstOrDefaultAsync(p => p.Id == id);

        if (processingJob == null)
        {
            return ServiceResult<ProcessingJob>.Fail(ErrorConstants.DocumentNotFound);
        }

        return ServiceResult<ProcessingJob>.Ok(processingJob);
    }

    public async Task<ServiceResult<ProcessingJob>> CreateJobRetry(int id)
    {
        var processingJob = await _traineeContext.ProcessingJobs.FirstOrDefaultAsync(p => p.Id == id);

        if (processingJob == null)
        {
            return ServiceResult<ProcessingJob>.Fail(ErrorConstants.DocumentNotFound);
        }

        processingJob.Status = "Queued";
        processingJob.Attempts = 0;
        processingJob.ErrorSummary = "";

        // publish message
        SubmissionProcessingRequestModel submissionProcessingRequestModel = new SubmissionProcessingRequestModel
        {
            CorrelationId = processingJob.CorrelationId,
            MessageId = processingJob.MessageId,
            RequestedAt = DateTime.UtcNow,
            SubmissionId = 0,
            SubmissionFileId = processingJob.SubmissionFileId,
        };
        await _traineeContext.SaveChangesAsync();
        
        // publish message
        await _rabbitMQPublisher.PublishMessageAsync(submissionProcessingRequestModel, RabbitMQQueues.SubmissionProcessingQueue);


        return ServiceResult<ProcessingJob>.Ok(processingJob);
    }
}