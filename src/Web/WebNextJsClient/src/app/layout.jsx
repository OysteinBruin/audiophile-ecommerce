import { Manrope } from 'next/font/google';
import Header from './components/Header';
import Footer from "./components/Footer";
import './styles/globals.scss';

const manrope = Manrope({
  weight: ['200', '400', '500', '700'],
  subsets: ['latin'],
});

export const metadata = {
  title: 'Audiophile',
  description: 'E-Commerce web shop for audio equipment',
  keywords:
      'speakers, headphones, earphones, audio, equipment, e-commerce, web shop',
};

export default function RootLayout({ children }) {
  return (
      <html lang='en'>
      <body className={manrope.className}>
      <Header />
      <main className='container'>{children}</main>
      <Footer />
      </body>
      </html>
  );
}
