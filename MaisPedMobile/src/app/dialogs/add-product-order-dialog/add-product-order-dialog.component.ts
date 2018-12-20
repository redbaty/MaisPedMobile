import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {Product} from '../../models/product';
import {IonSearchbar, ModalController} from '@ionic/angular';

@Component({
               selector: 'app-add-product-order-dialog',
               templateUrl: './add-product-order-dialog.component.html',
               styleUrls: ['./add-product-order-dialog.component.scss']
           })
export class AddProductOrderDialogComponent implements OnInit {
    public get searchFilter(): string {
        return this._searchFilter;
    }

    public set searchFilter(value: string) {
        this._searchFilter = value;

        if (this.searchBar) {
            this.searchBar.value = value;
        }

        this.updateFiltered();
    }

    @Input()
    public products: Product[];
    public filteredProducts: Product[];

    @ViewChild(IonSearchbar)
    public searchBar: IonSearchbar;

    private _searchFilter: string;

    constructor(public modalController: ModalController) {
    }

    ngOnInit() {
        this.updateFiltered();
    }

    public updateFiltered() {
        if (!this.searchFilter || this.searchFilter === '') {
            this.filteredProducts = this.products;
        } else {
            if (isNaN(+this.searchFilter)) {
                this.filteredProducts = this.products.filter(i => i.name.toUpperCase()
                                                                   .startsWith(this.searchFilter.toUpperCase()));
            } else {
                this.filteredProducts = this.products.filter(i => i.id.toString()
                                                                   .startsWith(this.searchFilter));
            }
        }
    }

    public searchChanged($event: CustomEvent) {
        if ($event.detail) {
            this.searchFilter = $event.detail.value;
        }
    }
}
