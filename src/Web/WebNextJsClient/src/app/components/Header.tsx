import Link from 'next/link';
import styles from '../styles/layout/header.module.scss'


const Header = () => {
    return (
        <header className='header'>
            <div className='container'>
                <div className='logo'>
                    <Link href='/'>Audiophile</Link>
                </div>
                <div className='links'>
                    <Link href='/'>Home</Link>
                    <Link href='/about'>About</Link>
                </div>
            </div>
        </header>
    );
};
export default Header;