function fetchAuthorName(){
  let authorId = document.getElementById("authorId").value;
  fetch(`/Authors/GetAuthorName/${authorId}`, {
    method: 'GET',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    }
  })
  .then(response => response.json())
  .then(response => {
    let author = JSON.parse(response);
    let nameField = document.getElementById('name-field');
    nameField.innerText = author.Name;
  })
}

document.getElementById("authorId").addEventListener("change", e=> {
  let authorId = document.getElementById("authorId").value;
  if (authorId) {
    fetchAuthorName();
  }
})