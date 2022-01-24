    // src/App.jsx

    import React, { Component } from 'react';
    import Polls from './components/polls';
    import Options from './components/options';

    class App extends Component {

      constructor(props) 
      {
        super(props);
        this.getPolls = this.getPolls.bind(this);
        this.getPollbyID = this.getPollbyID.bind(this);
      }

      state = {
        polls: [],
        options: []
      }

      getPollbyID()
      {
        let pollId = document.getElementById("inputPollID").value;
        fetch('http://localhost:8081/polls/' + pollId)
        .then(res => res.json())
        .then(({options}) => {
          this.setState({options})
          })
          .catch((err) => {
            console.log(err);
        })
      }

      getPolls()
      {
        fetch('http://localhost:8081/polls')
        .then(res => res.json())
        .then(({polls}) => {
          this.setState({polls})
          })
          .catch((err) => {
            console.log(err);
        });
      }

      postVote()
      {
        let pollID = document.getElementById("inputPollIDVote").value;
        let OptionID = document.getElementById("inputOptionIDVote").value;
        fetch("http://localhost:8081/polls/" + pollID + "/vote/" + OptionID, {
          method: "POST",
        })
      }

      clearOptions()
      {
        var container = document.getElementById("optionsDiv");
        // Clear previous contents of the container
        while (container.hasChildNodes()) {
            container.removeChild(container.lastChild);
        }
      }

      createPoll()
      {
        const optionsArray = []
        // create a `NodeList` object
        let opts = document.getElementsByName("pollOption")

        if(opts.length >= 2)
        {
          opts.forEach(element => {
            optionsArray.push(element.value)
          });
  
          let polltitle = document.getElementById("pollTitle").value;
          fetch("http://localhost:8081/polls/add", {
            mode: 'cors',
            // headers: {'Content-Type': 'application/json'}, 
            body: JSON.stringify(
            {
              "title": polltitle,
              "options":optionsArray
            }),
            method: "POST",
          })
        }
        else
        {
          alert("Not enough options, Please insert atleast 2 options")
        }
      }

      insertOption()
      {
        // Container <div> where dynamic content will be placed
        var container = document.getElementById("optionsDiv");
        // Append a node with a random text
        container.appendChild(document.createTextNode("Option"));
        // Create an <input> element, set its type and name attributes
        var input = document.createElement("input");
        input.type = "pollOption";
        input.name = "pollOption";
        container.appendChild(input);
        // Append a line break 
        container.appendChild(document.createElement("br"));
      }

      render() {
        return (
          <>
          <div class="grid-container">
            <div class="grid-item">
              <button onClick={this.getPolls}>Show Polls</button>
              <Polls polls={this.state.polls} />
            </div>

            <div class="grid-item">
              <button onClick={this.getPollbyID}>Get Poll with ID</button>
              <input class="number-input" id='inputPollID' type="number" defaultValue={1}></input>
              <Options options={this.state.options} />
            </div>

            <div class="grid-item">
              <button onClick={this.postVote}>PostVote</button>
              <p>Poll ID</p>
              <input class="number-input" id='inputPollIDVote' type="number" defaultValue={1}></input>
              <p>Vote ID</p>
              <input class="number-input" id='inputOptionIDVote' type="number" defaultValue={1}></input>
            </div>
            <div class="grid-item" id='createPollDiv'>
              <button onClick={this.clearOptions}>Clear options</button>
              <button onClick={this.insertOption}>Add option field</button>
              <button onClick={this.createPoll}>Create a poll</button>
              <input id='pollTitle' type="text" defaultValue={"Insert poll title here"}></input>
              <div id='optionsDiv'></div>
            </div>
          </div>

          </>
        )
      }
    }
    export default App;