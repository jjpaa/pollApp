// src/components/options.jsx

import React from 'react'

const Options = ({ options }) => {

  return (
    <div>
        <table border="2">
            <tbody>
                <tr>
                    <th>Id</th>
                    <th>Title</th>
                    <th>Votes</th>
                </tr>
                {options.map((item, i) => (
                    <tr key={i}>
                        <td>{item.id}</td>
                        <td>{item.title}</td>
                        <td>{item.votes}</td>
                    </tr>
                ))}
            </tbody>
        </table>
    </div>
  )
};

export default Options