import styles from '../styles/layout/footer.module.scss'
import Image from "next/image";
import classNames from "classnames";

const Footer = () => {
    return (
        <footer>
            <section className={styles.footer__aboutSection}>
                
                <div className={classNames("container", styles.footer__aboutContainer)}>
                    <picture>
                        <source media="(min-width: 1000px)"
                                srcSet="/img/shared/desktop/image-best-gear.jpg"/>
                        <source media="(min-width: 700px)"
                                srcSet="/img/shared/tablet/image-best-gear.jpg"/>
                        <img className={styles.footer__image}
                             src="/img/shared/mobile/image-best-gear.jpg"
                             alt="man with headphones"/>
                    </picture>
                    <div className={styles.footer__textBlock}>
                        <h2 className={styles.footer__h2}>
                            Bringing you the <span>best</span> audio gear
                        </h2>
                        <p className={styles.footer__p}>
                            Located at the heart of New York City, Audiophile is
                            the premier store for high end headphones,
                            earphones, speakers, and audio accessories. We have
                            a large showroom and luxury demonstration rooms
                            available for you to browse and experience a wide
                            range of our products. Stop by our store to meet
                            some of the fantastic people who make Audiophile the
                            best place to buy your portable audio equipment.
                        </p>
                    </div>
                </div>
            </section>

            <section className={classNames(styles.footer__infoSection)}>
                <div className={classNames("container", styles.footer__infoInner)}>
                    <div className={classNames("container", styles.footer__nav)}>
                        <img
                            src="./img/shared/desktop/logo.svg"
                            alt="audiophile"
                        />
                        <ul className={styles.footer__links}>
                            <li>
                                <a
                                    href="./index.html"
                                    className={classNames("container", "nav__link--current-page")}
                                >home</a
                                >
                            </li>
                            <li>
                                <a href="./headphones.html" className={styles.nav__link}
                                >headphones</a
                                >
                            </li>
                            <li>
                                <a href="./speakers.html" className={styles.nav__link}
                                >speakers</a
                                >
                            </li>
                            <li>
                                <a href="./earphones.html" className={styles.nav__link}
                                >earphones</a
                                >
                            </li>
                        </ul>
                    </div>

                    <div className={classNames("container", "footer__info")}>
                        <p className={classNames("footer__p", "footer__p--info", "text--white")}>
                            Audiophile is an all in one stop to fulfill your
                            audio needs. We&apos;re a small team of music lovers and
                            sound specialists who are devoted to helping you get
                            the most out of personal audio. Come and visit our
                            demo facility - we’re open 7 days a week.
                        </p>
                        <p className={classNames("footer__copyright", "text--white")}>
                            Copyright 2023. All Rights Reserved
                        </p>
                        <ul className={styles.footer__social}>
                            <li>
                                <a
                                    href="https://www.facebook.com/"
                                    target="_blank"
                                >
                                    <img
                                        src="/img/shared/desktop/icon-facebook.svg"
                                        alt="facebook"
                                        className={styles.footer__icon}
                                    />
                                </a>
                            </li>
                            <li>
                                <a
                                    href="https://www.twitter.com/"
                                    target="_blank"
                                >
                                    <img
                                        src="/img/shared/desktop/icon-twitter.svg"
                                        alt="twitter"
                                        className={styles.footer__icon}
                                    /></a>
                            </li>
                            <li>
                                <a
                                    href="https://www.instagram.com/"
                                    target="_blank"
                                >
                                    <img
                                        src="/img/shared/desktop/icon-instagram.svg"
                                        alt="instagram"
                                        className={styles.footer__icon}
                                    /></a>
                            </li>
                        </ul>
                    </div>
                </div>
            </section>
        </footer>
    )
}
export default Footer
