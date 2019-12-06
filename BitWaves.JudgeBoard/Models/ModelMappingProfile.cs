using AutoMapper;
using BitWaves.Data.Entities;
using BitWaves.JudgeBoard.Services;

namespace BitWaves.JudgeBoard.Models
{
    /// <summary>
    /// Provide AutoMapper profile for mapping models.
    /// </summary>
    public class ModelMappingProfile : Profile
    {
        /// <summary>
        /// Initialize a new <see cref="ModelMappingProfile"/>. This constructor will initializes all available mappings
        /// from source types to models.
        /// </summary>
        public ModelMappingProfile()
        {
            CreateOutputModelMappings();
            CreateInputModelMappings();
            CreateUpdateModelMappings();
        }

        /// <summary>
        /// Create model mappings for output models.
        /// </summary>
        private void CreateOutputModelMappings()
        {
            CreateMap<JudgeNodeInfo, JudgeNodeInfoModel>();
            CreateMap<Submission, SubmissionModel>()
                .ForMember(m => m.ArchiveId, opt => opt.Ignore());
            CreateMap<Language, LanguageModel>();
        }

        /// <summary>
        /// Create model mappings for input models.
        /// </summary>
        private void CreateInputModelMappings()
        {
            CreateMap<SubmissionJudgeResultModel, JudgeResult>();
            CreateMap<TestCaseJudgeResultModel, TestCaseResult>();
        }

        /// <summary>
        /// Create model mappings for update models.
        /// </summary>
        private void CreateUpdateModelMappings()
        {
            CreateMap<PatchJudgeNodeInfoModel, JudgeNodeInfo>()
                .ForMember(i => i.Status, opt => opt.Ignore());
        }
    }
}
