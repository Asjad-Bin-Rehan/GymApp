using DA.AppDbContexts;
using DM.DomainModels;
using GenericRepository;

namespace DA
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
        }

        public IGenericRepository<Location, string> location => new GenericRepository<Location, string>(_db);
        public IGenericRepository<Location_From_Location, string> location_from_location => new GenericRepository<Location_From_Location, string>(_db);

        public IGenericRepository<Encrypted_Credentials, string> encrypted_credentials => new GenericRepository<Encrypted_Credentials, string>(_db);
        public IGenericRepository<Production_QC, string> production_qc => new GenericRepository<Production_QC, string>(_db);
        public IGenericRepository<Production_QC_Sample, string> production_qc_sample => new GenericRepository<Production_QC_Sample, string>(_db);
        public IGenericRepository<Production_QC_Sample_Result, string> production_qc_sample_result => new GenericRepository<Production_QC_Sample_Result, string>(_db);
        public IGenericRepository<Purchase_QC, string> purchase_qc => new GenericRepository<Purchase_QC, string>(_db);
        public IGenericRepository<Purchase_QC_Sample, string> purchase_qc_sample => new GenericRepository<Purchase_QC_Sample, string>(_db);
        public IGenericRepository<Purchase_QC_Sample_Result, string> purchase_qc_sample_result => new GenericRepository<Purchase_QC_Sample_Result, string>(_db);
        public IGenericRepository<Inspection_Attribute, string> inspection_attribute => new GenericRepository<Inspection_Attribute, string>(_db);
        public IGenericRepository<Inspection_Characteristic, string> inspection_characteristic => new GenericRepository<Inspection_Characteristic, string>(_db);
        public IGenericRepository<Inspection_Characteristic_Mapping, string> inspection_characteristic_mapping => new GenericRepository<Inspection_Characteristic_Mapping, string>(_db);
        public IGenericRepository<Inspection_Card, string> inspection_card => new GenericRepository<Inspection_Card, string>(_db);
        public IGenericRepository<Item_Inspection_Card, string> item_inspection_card => new GenericRepository<Item_Inspection_Card, string>(_db);
        public IGenericRepository<Item_Sample, string> item_sample => new GenericRepository<Item_Sample, string>(_db);
        public IGenericRepository<Item, string> item => new GenericRepository<Item, string>(_db);
        public IGenericRepository<Qualitative_Result, string> qualitative_result => new GenericRepository<Qualitative_Result, string>(_db);
        public IGenericRepository<Qualitative_Result_Pass_Status, string> qualitative_result_pass_status => new GenericRepository<Qualitative_Result_Pass_Status, string>(_db);
        public IGenericRepository<Qualitative_Inspection, string> qualitative_inspection => new GenericRepository<Qualitative_Inspection, string>(_db);
        public IGenericRepository<Qualitative_Inspection_Mapping, string> qualitative_inspection_mapping => new GenericRepository<Qualitative_Inspection_Mapping, string>(_db);
        public IGenericRepository<Quantitative_Inspection, string> quantitative_inspection => new GenericRepository<Quantitative_Inspection, string>(_db);
        public IGenericRepository<Quantitative_Inspection_Mapping, string> quantitative_inspection_mapping => new GenericRepository<Quantitative_Inspection_Mapping, string>(_db);
        public IGenericRepository<Sampling_Range, string> sampling_range => new GenericRepository<Sampling_Range, string>(_db);
        public IGenericRepository<Unit_Of_Measure, string> unit_of_measure => new GenericRepository<Unit_Of_Measure, string>(_db);
        public IGenericRepository<Production_QA, string> production_qa => new GenericRepository<Production_QA, string>(_db);
        public IGenericRepository<Production_QA_Cavity, string> production_qa_cavity => new GenericRepository<Production_QA_Cavity, string>(_db);
        public IGenericRepository<Production_QA_Cavity_Sample, string> production_qa_cavity_sample => new GenericRepository<Production_QA_Cavity_Sample, string>(_db);
        public IGenericRepository<Production_QA_Cavity_Sample_Result, string> production_qa_cavity_sample_result => new GenericRepository<Production_QA_Cavity_Sample_Result, string>(_db);
        public IGenericRepository<Log_Sampling_Range, string> log_sampling_range => new GenericRepository<Log_Sampling_Range, string>(_db);
        public IGenericRepository<Log_Production_QA_Cavity, string> log_production_qa_cavity => new GenericRepository<Log_Production_QA_Cavity, string>(_db);
        public IGenericRepository<Log_Production_QA_Cavity_Sample_Result, string> log_production_qa_cavity_sample_result => new GenericRepository<Log_Production_QA_Cavity_Sample_Result, string>(_db);
        public IGenericRepository<Log_Production_QC_Sample_Result, string> log_production_qc_sample_result => new GenericRepository<Log_Production_QC_Sample_Result, string>(_db);
        public IGenericRepository<Log_Purchase_QC_Sample_Result, string> log_purchase_qc_sample_result => new GenericRepository<Log_Purchase_QC_Sample_Result, string>(_db);
        public IGenericRepository<Log_Post_Sap, string> log_post_sap => new GenericRepository<Log_Post_Sap, string>(_db);

        #region Commit
        public void Commit()
        {
            _db.SaveChanges();
        }
        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await _db.SaveChangesAsync(cancellationToken);
        }
        public async Task CommitAsync()
        {
            await _db.SaveChangesAsync();
        }
        public virtual void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);

        }
        #endregion Commit
    }
}
