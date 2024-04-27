using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Clients
{
    public class Client {
        private string _firstname;
        private string _lastname;
        private double _weight;
        private double _height;

        public Client()
        {
            FirstName = "xx";
            LastName = "xx";
            Weight = 0;
            Height = 0;
            
        }
        public Client(string firstname, string lastname, double weight, double height)
        {
            FirstName = firstname;
            LastName = lastname;
            Weight = weight;
            Height = height;
           
        }
        public string FirstName { get{ return _firstname;} 
        set {
            if (string.IsNullOrWhiteSpace(value)) {
            throw new ArgumentNullException("First Name is required. Must not be empty or blank.");
            }
            else {
                _firstname = value;
            }
				
        } 
        }
        public string LastName { get{ return _lastname;} 
        set {
            if (string.IsNullOrWhiteSpace(value)) {
            throw new ArgumentNullException("Last Name is required. Must not be empty or blank.");
            }
            else {
                _lastname = value;
            }
				
        } 
        }
        public double Weight {
			get { return _weight; }
			set {
				if (value < 0.0) {
					throw new ArgumentException("Weight must be greater than zero (0).");
        } else {
          _weight = value;
        }
			}
		}
        public double Height {
			get { return _height; }
			set {
				if (value < 0.0) {
					throw new ArgumentException("Weight must be greater than zero (0).");
        } else {
          _height = value;
        }
			}
		}
        public double BmiScore {
      get {
        double bmiScore = (Weight / (Height * Height) * 703);
        return bmiScore;
      }
    }
    public string BmiStatus {
      get {
        string bmiStatus = "";
        double bmiScore = BmiScore;

        if(bmiScore >= 0 && bmiScore <= 18.4) {
          bmiStatus = "Underweight";
        } else if(bmiScore >= 18.5 && bmiScore <= 24.99) {
          bmiStatus = "Normal";
        } else if(bmiScore >= 25 && bmiScore <= 39.99) {
          bmiStatus = "Overweight";
        } else {
          bmiStatus = "Obese";
        }
        return bmiStatus;
      }
    }
    public override string ToString() {
			return $"{FirstName},{LastName},{Weight},{Height}";
		}

    }
}



