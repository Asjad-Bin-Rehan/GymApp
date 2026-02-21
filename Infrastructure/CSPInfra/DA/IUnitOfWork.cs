using DM.DomainModels;
using GenericRepository;

namespace DA
{
    public interface IUnitOfWork
    {
        IGenericRepository<Location, string> location { get; }
        IGenericRepository<Location_From_Location, string> location_from_location { get; }

        IGenericRepository<Encrypted_Credentials, string> encrypted_credentials { get; }
        IGenericRepository<Inspection_Attribute, string> inspection_attribute { get; }
        IGenericRepository<Inspection_Characteristic, string> inspection_characteristic { get; }
        IGenericRepository<Inspection_Characteristic_Mapping, string> inspection_characteristic_mapping { get; }
        IGenericRepository<Inspection_Card, string> inspection_card { get; }
        IGenericRepository<Item, string> item { get; }
        IGenericRepository<Item_Inspection_Card, string> item_inspection_card { get; }
        IGenericRepository<Item_Sample, string> item_sample { get; }
        IGenericRepository<Log_Post_Sap, string> log_post_sap { get; }
        IGenericRepository<Log_Sampling_Range, string> log_sampling_range { get; }
        IGenericRepository<Log_Production_QA_Cavity, string> log_production_qa_cavity { get; }
        IGenericRepository<Log_Production_QA_Cavity_Sample_Result, string> log_production_qa_cavity_sample_result { get; }
        IGenericRepository<Log_Production_QC_Sample_Result, string> log_production_qc_sample_result { get; }
        IGenericRepository<Log_Purchase_QC_Sample_Result, string> log_purchase_qc_sample_result { get; }
        IGenericRepository<Qualitative_Inspection, string> qualitative_inspection { get; }
        IGenericRepository<Qualitative_Inspection_Mapping, string> qualitative_inspection_mapping { get; }
        IGenericRepository<Qualitative_Result, string> qualitative_result { get; }
        IGenericRepository<Qualitative_Result_Pass_Status, string> qualitative_result_pass_status { get; }
        IGenericRepository<Quantitative_Inspection, string> quantitative_inspection { get; }
        IGenericRepository<Quantitative_Inspection_Mapping, string> quantitative_inspection_mapping { get; }
        IGenericRepository<Sampling_Range, string> sampling_range { get; }
        IGenericRepository<Unit_Of_Measure, string> unit_of_measure { get; }
        IGenericRepository<Production_QC, string> production_qc { get; }
        IGenericRepository<Production_QC_Sample, string> production_qc_sample { get; }
        IGenericRepository<Production_QC_Sample_Result, string> production_qc_sample_result { get; }
        IGenericRepository<Purchase_QC, string> purchase_qc { get; }
        IGenericRepository<Purchase_QC_Sample, string> purchase_qc_sample { get; }
        IGenericRepository<Purchase_QC_Sample_Result, string> purchase_qc_sample_result { get; }
        IGenericRepository<Production_QA, string> production_qa { get; }
        IGenericRepository<Production_QA_Cavity, string> production_qa_cavity { get; }
        IGenericRepository<Production_QA_Cavity_Sample, string> production_qa_cavity_sample { get; }
        IGenericRepository<Production_QA_Cavity_Sample_Result, string> production_qa_cavity_sample_result { get; }

        void Commit();
        Task CommitAsync(CancellationToken cancellationToken);
        Task CommitAsync();
    }
}
