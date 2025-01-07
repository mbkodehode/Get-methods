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

})
    .catch((error) => {
        console.error("Error fetching all people:", error);
    });
}

function addNewPerson() {
    console.log("Adding new person");
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