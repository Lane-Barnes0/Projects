let pokemonNameRef = document.getElementById("pokemon-name");
let searchButton = document.getElementById("search-button");
let result = document.getElementById("result");

let getAbilities = (abilities) => {
    answer = "Abilities: "
    for(let i = 0; i < abilities.length; i++) {
        answer += abilities[i].ability.name + ", ";
    }
    return answer.slice(0,-2)
}


let getPokemon = () => {
    
    let pokemonName = pokemonNameRef.value.toLowerCase();
    
    let url = `https://pokeapi.co/api/v2/pokemon/${pokemonName}/`;

    if (pokemonName.length <= 0) {
        result.innerHTML = `<h3 class="msg">Please enter a pokemon name </h3>`;
    }

    else {
        fetch(url).then((resp) => resp.json()).then((data) => {
            //if Pokemon exists
            console.log(data)
                result.innerHTML = `
                    <div class="info">
                        
                        <div>
                            <img src =${data.sprites.front_default} class = "picture">
                            
                            <h2>${getAbilities(data.abilities)}</h2>
                            <div class="details">
                                <h3>Height: ${data.height} Decimeters</h3>
                                <h3>Weight: ${data.weight} Hectograms</h3>
                                
                                <h3>Type: ${data.types[0].type.name}</h3>
                            </div>   
                        </div>
                    </div>
                `;
            
        })
            //If Pokemon was not found
            .catch(() => {
                result.innerHTML = `<h3 class="msg">Pokemon Not Found</h3>`;
            });
    }
};


searchButton.addEventListener("click", getPokemon);
window.addEventListener("load", getPokemon);
