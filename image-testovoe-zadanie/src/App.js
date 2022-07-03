
import React from "react";
import './App.css';
import ImageList from './components/ImageList';
import OneImage from './components/OneImage';
import {HashRouter, Routes, Route} from 'react-router-dom'


function App() {
  return (
    <div className='container'>
    <HashRouter>
      <Routes>

        <Route path="/" element={<ImageList/>} />
        <Route path="/ImageList" element={<ImageList/>} />
        <Route path="/oneimage" element={<OneImage/>} />

      </Routes>
    </HashRouter>
    </div>
   
  );
}

export default App;
