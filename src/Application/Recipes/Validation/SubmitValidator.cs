using System.Text;

namespace Application.Recipes.Validation
{
	internal class SubmitValidator
	{
		internal string ValidateSubmit(int dishTypeId, Stream stream, string name)
		{
			StringBuilder validationResultBuilder = new StringBuilder();

			if (dishTypeId == 0) validationResultBuilder.Append("dishtype");
			if (stream == null) validationResultBuilder.Append("file");
			if (name is null || name.Trim() == string.Empty) validationResultBuilder.Append("name");
			if (validationResultBuilder.Length > 0) {return validationResultBuilder.ToString();}

			return "done";
		}
	}
}
