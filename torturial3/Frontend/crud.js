// Clientside script for the CRUD operations

function getAllPeople() {
    console.log("Getting all people");
    // TODO: Fetch all people from the backend and display them in the table on the frontend
    fetch("http://localhost:5220/people",{
        
            method: "GET"
        })
    .then((response)=> {
        if (!response.ok) {
            throw new Error("Failed to fetch all people from the backend");
        }
        return response.json();
    })
    .then((data) => {
        //to see if we can fetch the data
        console.log(data);
        //our logic to display the data in the table
        // grab the table body element
        const peopleList = document.getElementById("peopleList");
        // clear the table body and create a new table
        peopleList.innerHTML = "";

        // loop through all people and add them to the table
        data.forEach(person => {
            const row = document.createElement("tr");
            //create a cell for each column
            //person id
            const idCell = document.createElement("td");
            idCell.textContent = person.id;
            row.appendChild(idCell);

            //person name
            const nameCell = document.createElement("td");
            nameCell.textContent = person.name;
            row.appendChild(nameCell);

            //person age
            const ageCell = document.createElement("td");
            ageCell.textContent = person.age;
            row.appendChild(ageCell);

            peopleList.appendChild(row);
          
        });
})
    .catch((error) => {
        console.error("Error fetching all people:", error);
    });
}

function addNewPerson() {
    console.log("Adding new person");

    const name = document.getElementById("name").value;
    const age = document.getElementById("age").value;
    const city = document.getElementById("city").value;

    if (!name || !age || !city) {
        console.error("Name, age, and city are required");
        return;
    }

    const formData = new FormData();
    formData.append("name", name);
    formData.append("age", age);
    formData.append("city", city);

    fetch("http://localhost:5220/addperson", {
        method: "POST",
        body: formData,
    })
    .then((response) => {
        if (!response.ok) {
            throw new Error("Failed to add new person");
        }
        return response.json(); // parse the response body as json
    })
    .then((data) => {
        console.log(data);
        // refresh the table to show the new person added to the table
        getAllPeople();
        //reset the form
        document.querySelector("#name").value = "";
        document.querySelector("#age").value = "";
        document.querySelector("#city").value = "";
    })
    .catch((error) => {
        console.error("Error adding new person:", error);
    });
}

function findPerson() {
    console.log("Script for finding person");
    let Id =parseInt(document.getElementById("findId").value);
    if (isNaN(Id)) {
        console.error("Id is required to be a number");
        return;
    }
    const formData = new FormData();
    formData.append("id", Id);

    // fetch person by id
    fetch("http://localhost:5220/findperson", {
        method: "POST",
        body: formData,
    })
    .then((response) => {
        if (!response.ok) {
            throw new Error("Failed to find person");
        }
        return response.json(); // parse the response body as json
    })
    .then((data) => {
        console.log(data);
        // refresh the table to show the new person added to the table
        alert('Person found: ' + data.name + ' ' + data.age + ' ' + data.city);
        //reset the form

    })
    .catch((error) => {
        console.error("Error finding person:", error);
    });
}
function updatePerson() {
    console.log("Updating person");
}
function deletePerson() {
    console.log("Deleting person");
}

function init() {
    console.log("Initializing CRUD frontend");
    getAllPeople();
}