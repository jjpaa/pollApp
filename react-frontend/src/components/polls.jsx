// src/components/polls.jsx

import React from 'react'

const Polls = ({ polls }) => {
  return (
    <div>
        <table border="2">
            <tbody>
                <tr>
                    <th>Id</th>
                    <th>Title</th>
                </tr>
                {polls.map((item, i) => (
                    <tr key={i}>
                        <td>{item.id}</td>
                        <td>{item.title}</td>
                    </tr>
                ))}
            </tbody>
        </table>
    </div>
  )
};

export default Polls