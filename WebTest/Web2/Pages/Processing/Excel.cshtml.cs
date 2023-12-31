using IronXL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web2.Models;

namespace Web2.Pages
{
    public class ExcelModel : PageModel
    {
        private readonly Web2.Data.AppDbContext _context;
        [BindProperty]
        public List<List<string>> Data { get; set; }
        public ExcelModel(Web2.Data.AppDbContext context)
        {
            _context = context;
            Data = new List<List<string>>();
        }
        // ������ ��� ������ ������ ��
        public patient_archive Patient { get; set; }
        public Analyzes AnalyzesP { get; set; }
        public Examination ExaminationP { get; set; }
        public Archive_Group Archive { get; set; }
        public Patient_Analyzes PA { get; set; }
        public Patient_Examination PE { get; set; }
        // ����� ������� ��
        public List<SelectListItem>? GetName()
        {
            List<SelectListItem> group = new List<SelectListItem>();
            group.Add(new SelectListItem() { Text = "����� ���������", Value = "patient_archive" });
            group.Add(new SelectListItem() { Text = "�������", Value = "analyzes" });
            group.Add(new SelectListItem() { Text = "���������� ���������������", Value = "examination" });
            group.Add(new SelectListItem() { Text = "�����: ��������-������", Value = "archive_group" });
            group.Add(new SelectListItem() { Text = "�����: ��������-�������", Value = "patient_analyzes" });
            group.Add(new SelectListItem() { Text = "�����: ��������-���������������", Value = "patient_examinations" });
            return group;
        }
        public void OnGet(){}
        // ���� ������ �� Excel ����� � ��
        public async Task<IActionResult> OnPostAdd(string table_name)
        {
            try
            {
                switch (table_name)
                {
                    case "patient_archive":
                        if (Data.Count == 6) {
                            for (int i = 0; i < Data[0].Count; i++)
                            {
                                Patient = new patient_archive();
                                Patient.name = Data[1][i];
                                Patient.surname = Data[2][i];
                                Patient.patronymic = Data[3][i];
                                var dateTime = DateTime.Parse(Data[4][i]);
                                Patient.birthday = DateOnly.FromDateTime(dateTime);
                                Patient.phone_num = Data[5][i];

                                _context.patient_archive.Add(Patient);
                                await _context.SaveChangesAsync();
                            }
                            break;
                        }
                        ModelState.AddModelError("", "�������� ������");
                        break;
                    case "analyzes":
                        if (Data.Count == 8)
                        {
                            for (int i = 0; i < Data[0].Count; i++)
                            {
                                AnalyzesP = new Analyzes();
                                AnalyzesP.serum_calcium = Convert.ToDouble(Data[1][i]);
                                AnalyzesP.serum_phosphorus = Convert.ToDouble(Data[2][i]);
                                AnalyzesP.serum_oxyproline = Convert.ToDouble(Data[3][i]);
                                AnalyzesP.calcium_excretion = Convert.ToDouble(Data[4][i]);
                                AnalyzesP.phosphorus_excretion = Convert.ToDouble(Data[5][i]);
                                AnalyzesP.oxyproline_excretion = Convert.ToDouble(Data[6][i]);
                                AnalyzesP.severity_of_dst = Convert.ToInt32(Data[7][i]);

                                _context.analyzes.Add(AnalyzesP);
                                await _context.SaveChangesAsync();
                            }
                            break;
                        }
                        ModelState.AddModelError("", "�������� ������");
                        break;
                    case "examination":
                        if (Data.Count == 13)
                        {
                            for (int i = 0; i < Data[0].Count; i++)
                            {
                                ExaminationP = new Examination();
                                var dateTime = DateTime.Parse(Data[1][i]);
                                ExaminationP.date = DateOnly.FromDateTime(dateTime);
                                ExaminationP.posture_holding_time = Convert.ToDouble(Data[2][i]);
                                ExaminationP.the_amount_of_kyphosis_in_degress = Convert.ToDouble(Data[3][i]);
                                ExaminationP.changing_the_contours_of_the_end_plates = Convert.ToBoolean(Data[4][i]);
                                ExaminationP.wedgeshaped_vertebral_bodies = Convert.ToBoolean(Data[5][i]);
                                ExaminationP.schmorl_hernia = Convert.ToBoolean(Data[6][i]);
                                ExaminationP.osteoporosis_of_the_vertebrae = Convert.ToBoolean(Data[7][i]);
                                ExaminationP.decrease_in_the_height_of_the_intervertebral_disc = Convert.ToBoolean(Data[8][i]);
                                ExaminationP.change_in_the_contours_of_the_apophyses = Convert.ToBoolean(Data[9][i]);
                                ExaminationP.signs_of_osteochondrosis = Convert.ToBoolean(Data[10][i]);
                                ExaminationP.stabilographic_changes = Convert.ToBoolean(Data[11][i]);
                                ExaminationP.enmg = Convert.ToBoolean(Data[12][i]);

                                _context.analyzes.Add(AnalyzesP);
                                await _context.SaveChangesAsync();
                            }
                            break;
                        }
                        ModelState.AddModelError("", "�������� ������");
                        break;
                    case "archive_group":
                        if (Data.Count == 2)
                        {
                            for (int i = 0; i < Data[0].Count; i++)
                            {
                                Archive = new Archive_Group();
                                Archive.id_patient= Convert.ToInt32(Data[0][i]);
                                Archive.id_group= Convert.ToInt32(Data[1][i]);

                                _context.archive_group.Add(Archive);
                                await _context.SaveChangesAsync();
                            }
                            break;
                        }
                        ModelState.AddModelError("", "�������� ������");
                        break;
                    case "patient_analyzes":
                        if (Data.Count == 2)
                        {
                            for (int i = 0; i < Data[0].Count; i++)
                            {
                                PA = new Patient_Analyzes();
                                PA.id_patient = Convert.ToInt32(Data[0][i]);
                                PA.id_analysis = Convert.ToInt32(Data[1][i]);

                                _context.patient_analyzes.Add(PA);
                                await _context.SaveChangesAsync();
                            }
                            break;
                        }
                        ModelState.AddModelError("", "�������� ������");
                        break;
                    case "patient_examinations":
                        if (Data.Count == 2)
                        {
                            for (int i = 0; i < Data[0].Count; i++)
                            {
                                PE = new Patient_Examination();
                                PE.id_patient = Convert.ToInt32(Data[0][i]);
                                PE.id_examination = Convert.ToInt32(Data[1][i]);

                                _context.patient_examinations.Add(PE);
                                await _context.SaveChangesAsync();
                            }
                            break;
                        }
                        ModelState.AddModelError("", "�������� ������");
                        break;
                }
            }
            catch (Exception ex)
            {
                return Redirect("./Error");
            }
            //return Redirect("./Excel");
            Data.Clear();
            return Page();
        }
        // ��������� ������ �� Excel �����
        public Task<IActionResult> OnPostImport(IFormFile file)
        {
            if (file != null)
            {
                try
                {
                    Data = new List<List<string>>();
                    //Load your Excel file
                    var workbook = new WorkBook(file.OpenReadStream());
                    //Select your Worksheet
                    WorkSheet sheet = workbook.DefaultWorkSheet;
                    for (int i = 0; i < sheet.RowCount; i++)
                    {
                        var row = new List<string>();
                        for (int j = 0; j < sheet.ColumnCount; j++)
                        {
                            row.Add(sheet.GetCellAt(i, j).Value.ToString());
                        }
                        Data.Add(row);
                    }
                }
                catch (Exception ex)
                {
                    return Redirect("./Error");
                }
            }
            return Task.FromResult<IActionResult>(Page());
        }  
    }
}