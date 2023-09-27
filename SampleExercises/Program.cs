using SimpleDataManagement.Models;
using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

var dataSourcesDirectory = Path.Combine(Environment.CurrentDirectory, "DataSources");
var personsFilePath = Path.Combine(dataSourcesDirectory, "Persons_20220824_00.json");
var organizationsFilePath = Path.Combine(dataSourcesDirectory, "Organizations_20220824_00.json");
var vehiclesFilePath = Path.Combine(dataSourcesDirectory, "Vehicles_20220824_00.json");
var addressesFilePath = Path.Combine(dataSourcesDirectory, "Addresses_20220824_00.json");

//Quick test to ensure that all files are where they should be :)
foreach (var path in new[] { personsFilePath, organizationsFilePath, vehiclesFilePath, addressesFilePath })
{
    if (!File.Exists(path))
        throw new FileNotFoundException(path);
}

//TODO: Start your exercise here. Do not forget about answering Test #9 (Handled slightly different)
// Reminder: Collect the data from each file. Hint: You could use Newtonsoft's JsonConvert or Microsoft's JsonSerializer

List<Person> person;
List<Organization> organization;
List<Vehicle> vehicles;
List<Address> addresses;

using(StreamReader personFile = File.OpenText(personsFilePath))           
{
    var jsonPerson = personFile.ReadToEnd();
    person = JsonSerializer.Deserialize<List<Person>>(jsonPerson);
}

using (StreamReader organizationFile = File.OpenText(organizationsFilePath))
{
    var jsonOrganization = organizationFile.ReadToEnd();
    organization = JsonSerializer.Deserialize<List<Organization>>(jsonOrganization);
}

using (StreamReader vehicleFile = File.OpenText(vehiclesFilePath))
{
    var jsonVehicle = vehicleFile.ReadToEnd();
    vehicles = JsonSerializer.Deserialize<List<Vehicle>>(jsonVehicle);
}

using (StreamReader addressFile = File.OpenText(addressesFilePath))
{
    var jsonAddress = addressFile.ReadToEnd();
    addresses = JsonSerializer.Deserialize<List<Address>>(jsonAddress);
}




//Test #1: Do all files have entities? (True / False)
Console.WriteLine("Answer 1)");

if(person != null && person.Count > 0 && organization != null && organization.Count > 0 && vehicles != null && vehicles.Count > 0 && addresses != null && addresses.Count > 0)
{
    Console.WriteLine("Yes, all files have entities. So the answer is True");
}

else
{
    Console.WriteLine("No, one or more than one files does not contain entities. So the answer is False");
}

Console.WriteLine();


//Test #2: What is the total count for all entities?
Console.WriteLine("Answer 2)");

if (person != null && organization != null && vehicles != null && addresses != null)
{
    int sum = person.Count + organization.Count + vehicles.Count + addresses.Count;

    Console.WriteLine("Total count for all entities is: " + sum);
}

else
{
    Console.WriteLine("One or more of the entity is null so the total sum cannot be calculated");
}

Console.WriteLine();



//Test #3: What is the count for each type of Entity? Person, Organization, Vehicle, and Address
Console.WriteLine("Answer 3)");

Console.WriteLine("Count of Person: " + person.Count);
Console.WriteLine("Count of Organization: " + organization.Count);
Console.WriteLine("Count of Vehicle: " + vehicles.Count);
Console.WriteLine("Count of Address: " + addresses.Count);

Console.WriteLine();



//Test #4: Provide a breakdown of entities which have associations in the following manor:
//         -Per Entity Count
//         - Total Count
Console.WriteLine("Answer 4)");

int sumOfPersonAssociation = 0;
foreach(var a in person)
{
    sumOfPersonAssociation = sumOfPersonAssociation + a.Associations.Count;   
}
Console.WriteLine("Sum per entity count for Person: " + sumOfPersonAssociation);

int sumOfOrganizationAssociation = 0;
foreach(var b in organization)
{
    sumOfOrganizationAssociation = sumOfOrganizationAssociation + b.Associations.Count;
}
Console.WriteLine("Sum per entity count for Organization: " + sumOfOrganizationAssociation);

int sumOfVehicleAssociation = 0;
foreach(var c in vehicles)
{
    sumOfVehicleAssociation = sumOfVehicleAssociation + c.Associations.Count;
}
Console.WriteLine("Sum per entity count for Vehicle: " + sumOfVehicleAssociation);

int sumOfAddressAssociation = 0;
foreach(var d in addresses)
{
    sumOfAddressAssociation = sumOfAddressAssociation + d.Associations.Count;
}
Console.WriteLine("Sum per entity count fot Address: " + sumOfAddressAssociation);

int totalCount = sumOfPersonAssociation + sumOfOrganizationAssociation + sumOfVehicleAssociation + sumOfAddressAssociation;
Console.WriteLine("Total count: " + totalCount);

Console.WriteLine();



//Test #5: Provide the vehicle detail that is associated to the address "4976 Penelope Via South Franztown, NH 71024"?
//         StreetAddress: "4976 Penelope Via"
//         City: "South Franztown"
//         State: "NH"
//         ZipCode: "71024"
Console.WriteLine("Answer 5)");

string findStreetAddress = "4976 Penelope Via";
string findCity = "South Franztown";
string findState = "NH";
string findZipCode = "71024";

bool isAnswer = false;
foreach(var detail in addresses)
{
    
    if (detail.StreetAddress.Equals(findStreetAddress) && detail.City.Equals(findCity) && detail.ZipCode.Equals(findZipCode) && detail.State.Equals(findState))  
    {
      
        foreach(var asso in detail.Associations)
        {
            string associatedEntityNo = asso.EntityId;

            foreach (var e in vehicles)
            {

                if (e.EntityId.Equals(associatedEntityNo))
                {
                    Console.WriteLine("ID: " +  e.EntityId);
                    Console.WriteLine("Make: " + e.Make);
                    Console.WriteLine("Model: " + e.Model);
                    Console.WriteLine("Year: " + e.Year);
                    Console.WriteLine("Plate: " + e.PlateNumber);
                    Console.WriteLine("State: " + e.State);
                    Console.WriteLine("Vin: " + e.Vin);
                    isAnswer = true;
                    break;
                }
            }
        }
       
    }
}

if(isAnswer == false)
{
    Console.WriteLine("There is no such vehicle associated with the given address");
}

Console.WriteLine();



//Test #6: What person(s) are associated to the organization "thiel and sons"?
Console.WriteLine("Answer 6)");

bool foundAnswer = false;
foreach (var f in organization)
{
    if(f.Name.Equals("thiel and sons"))
    {
        foreach(var assoc in  f.Associations)
        {
            foreach(var per in person)
            {
                if(per.EntityId.Equals(assoc.EntityId))
                {
                    Console.WriteLine("First Name: " + per.FirstName);
                    Console.WriteLine("Middle Name: " + per.MiddleName);
                    Console.WriteLine("Last Name: " + per.LastName);
                    foundAnswer = true;
                    break;
                }
            }
        }
    }

}

if (foundAnswer == false)
{
    Console.WriteLine("No peron are associated with given organization");
}

Console.WriteLine();



//Test #7: How many people have the same first and middle name?
Console.WriteLine("Answer 7)");

//First Interpretation - find the person whose first name is same as his middle name (e.g John John Smith)
int ans = 0;
foreach(var m in person)
{
    if(m.MiddleName.Equals(m.FirstName))
    {
        ans++;
    }
}
Console.WriteLine("Total people having same first and middle name as per first interpretation: " + ans);


//Second Interpretation - find a person whose first name is equal of first name of another person and whose middle name is also equal to middle name of that another person
//(e.g John D Smith and John D Fletcher)
Dictionary<string, int> nameCount = new Dictionary<string, int>();

foreach(var r in person)
{
    string joinName = r.FirstName + r.MiddleName;

    if(nameCount.ContainsKey(joinName))
    {
        nameCount[joinName]++;
    }

    else
    {
        nameCount.Add(joinName, 1);
    }
}

int totalName = 0;

foreach(var dict in nameCount)
{
    if(dict.Value > 1)
    {
        totalName = totalName + dict.Value;
    }
}

Console.WriteLine("Total people having same first and middle name as per second interpretation: " + totalName);


Console.WriteLine();


//Test #8: Provide a breakdown of entities where the EntityId contains "B3" in the following manor:
//         -Total count by type of Entity
//         - Total count of all entities
Console.WriteLine("Answer 8)");

int count1 = 0;
foreach(var g in person)
{
    if(g.EntityId.Contains("b3") == true)
    {
        count1++;
    }
}
Console.WriteLine("Person: " + count1);

int count3 = 0;
foreach(var k in vehicles)
{
    if(k.EntityId.Contains("b3"))
    {
        count3++;
    }
}
Console.WriteLine("Vehicle: " + count3);

int count2 = 0;
foreach(var h in organization)
{
    if(h.EntityId.Contains("b3"))
    {
        count2++;
    }
}
Console.WriteLine("Organization: " + count2);

int count4 = 0;
foreach(var l in addresses)
{
    if(l.EntityId.Contains("b3"))
    {
        count4++;
    }
}
Console.WriteLine("Address: " + count4);

Console.WriteLine("Total count of all entities: " + (count1 + count2 + count3 + count4));


/*
 * Answer 9)
 * I would like to make the following improvements to the object model to improve the overall workflow without impacting the json files:
 * 
 * a) We can use the concept of Inheritance. In all these classes, EntityID and the collection Associations is common. So, instead of writing them out individually in all classes, we can create
 * another super class (name it anything we want). That superclass will contain fields EntityId and Associations. And then, the classes Person, Organization, Vehicle and Address will become
 * the sub class. Meaning that they will inherit variables, methods and properties from the superclass. That way, we will not need to define EntityId and Association individually in 
 * all these classes. It will help to reduce redundancy and make the code cleaner. 
 * 
 * b) I would like to initialize collection in the classes Person, Vehicle, Organization, and Address. Currently, the collection "Associations" in all these classes is not
 * initialized. We can initialize them in the class itself. Doing this will ensure that whenever we create an object of Person, Vehicle, Organization or Address class, "Associations" 
 * is always initialized to empty list. As a result, we won't have to always check it for null pointer exceptions. 
 * 
 * 
 * 
 */