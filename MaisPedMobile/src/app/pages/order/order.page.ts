import {Component, ViewChild} from '@angular/core';
import {PeopleService} from '../../services/people.service';
import {Person} from '../../models/person';
import {Product} from '../../models/product';
import {ProductsService} from '../../services/products.service';
import {IonSearchbar, IonSlides, ModalController} from '@ionic/angular';
import {Order} from '../../models/order';
import {AddProductOrderDialogComponent} from '../../dialogs/add-product-order-dialog/add-product-order-dialog.component';
import {List} from 'linqts';
import {OrderService} from '../../services/order.service';

enum OrderPageStatus {
    SelectingClient = 0,
    SeeingProducts = 1,
    Finalizing = 2
}

@Component({
               selector: 'app-order',
               templateUrl: './order.page.html',
               styleUrls: ['./order.page.scss'],
           })
export class OrderPage {
    @ViewChild('searchBar')
    public searchBar: IonSearchbar;
    public order: Order = {products: []};
    public isLoading = 0;
    public people: Person[] = [];
    public products: Product[] = [];
    public searchPlaceholder: string;
    public slidesColor = 'primary';
    public headerColor = 'primary';
    @ViewChild('slider')
    public slider: IonSlides;
    private lastSearch: string;
    private lastSearchResult: any[];

    constructor(
        private peopleService: PeopleService,
        private productsService: ProductsService,
        private modalController: ModalController,
        private orderService: OrderService
    )
    {
        this.peopleService.getAll()
            .then(people => {
                if (people) {
                    this.people = people;
                }

                this.isLoading++;
            });

        this.productsService.getAll()
            .then(products => {
                if (products) {
                    this.products = products;
                }

                this.isLoading++;
            });
    }

    public _searchFilter: string;

    public get searchFilter() {
        return this._searchFilter;
    }

    public set searchFilter(value: string) {
        this._searchFilter = value;

        if (this.searchBar) {
            this.searchBar.value = value;
        }
    }

    private _status: OrderPageStatus = OrderPageStatus.SelectingClient;

    public get status(): OrderPageStatus {
        return this._status;
    }

    public set status(value: OrderPageStatus) {
        this._status = value;
        this.lastSearchResult = undefined;
        this.lastSearch = undefined;
        this.searchFilter = '';
        this.searchBar.value = '';
    }

    public thingsChanged($event: CustomEvent) {
        if ($event.detail) {
            this.searchFilter = $event.detail.value;
        }
    }

    public getPeopleFiltered(filter: string) {
        if (!filter || filter === '') {
            return this.people;
        }

        if (this.lastSearch === filter) {
            return this.lastSearchResult;
        }

        this.lastSearch = filter;

        if (!isNaN(+filter)) {
            this.lastSearchResult = this.people.filter(oi => oi.id.toString()
                                                               .startsWith(filter));
        } else {
            this.lastSearchResult = this.people.filter(i => i.name.toUpperCase()
                                                             .startsWith(filter.toUpperCase()));
        }
        return this.lastSearchResult;
    }

    public getPlaceholder(tabIndex: number) {
        if (tabIndex === 0) {
            this.searchPlaceholder = 'Procurar cliente';
        } else {
            this.searchPlaceholder = 'Procurar produto';
        }
    }

    public sliderChanged() {
        this.slider.getActiveIndex()
            .then(newIndex => {
                this.status = newIndex;
            });
    }

    public getStatusTitle(status: OrderPageStatus) {
        switch (status) {
            case OrderPageStatus.SeeingProducts:
                return 'Seus produtos';
            case OrderPageStatus.SelectingClient:
                return 'Procurar selecionar';
            case OrderPageStatus.Finalizing:
                return 'Finalizando';
        }
    }

    public openAddProductDialog() {
        this.modalController.create({
                                        component: AddProductOrderDialogComponent,
                                        componentProps: {
                                            'products': this.products
                                        }
                                    })
            .then(modal => {
                modal.onDidDismiss()
                     .then(x => {
                         const val = x.data;
                         if (val) {
                             val.quantity = 1;
                             this.order.products.push(val);
                         }
                     });
                modal.present();
            });
    }

    public getOrderTotal(order: Order) {
        if (!order.products) {
            return 0;
        }

        return new List(order.products).Sum(i => i.fullPrice * i.quantity);
    }

    public endOrder(order: Order) {
        this.orderService.add(order)
            .then(x => {
                console.log('done');
            });
    }
}
