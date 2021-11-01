using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fedco.PHED.Agent.management.Web.Models
{
  public class RequestRecords
  {
    [RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "Invalid Name")]
    [Required(ErrorMessage = "Request is mandatory.")]
    [DisplayName("Request : ")]
    public string Request { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
    [Required(ErrorMessage = "Start date is mandatory.")]
    [DisplayName("Start Date : ")]
    public DateTime StartDateTime { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
    [Required(ErrorMessage = "End date is mandatory.")]
    [DisplayName("End Date : ")]
    public DateTime EndDateTime { get; set; }
  }
}
